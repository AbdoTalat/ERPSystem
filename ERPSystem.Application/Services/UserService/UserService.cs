using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ERPSystem.Application.DTOs.AppUser;
using ERPSystem.Application.IRepository;
using ERPSystem.Application.Services.TokenService;
using ERPSystem.Domain;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using Helper.API;
using Helper.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager,
            IUserRepository userRepository, RoleManager<AppRole> roleManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResponseHelper<IEnumerable<GetAllUsersDTO>>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Repository<AppUser>().GetAllAsDtoAsync<GetAllUsersDTO>();

            return ApiResponseHelper<IEnumerable<GetAllUsersDTO>>.ResponseSuccess(data: users);
        }
        public async Task<ApiResponseHelper<GetUserByIdDTO>> GetUserByIdAsync(int Id)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetByIdAsDtoAsync<GetUserByIdDTO>(Id);
            if (user == null)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"User not found with ID: {Id}");
            }
            return ApiResponseHelper<GetUserByIdDTO>.ResponseSuccess(data: user);
        }
        public async Task<ApiResponseHelper<GetUserByIdDTO>> AddUserAsync(AddUserDTO userDTO)
        {
            var IsUserExistByUserName = await _userManager.FindByNameAsync(userDTO.UserName);
            if (IsUserExistByUserName != null)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "This User name already exists");
            }

            var IsUserExistByEmail = await _userManager.FindByEmailAsync(userDTO.Email);
            if (IsUserExistByEmail != null)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "This User Email already exists");
            }

            if (!userDTO.BranchIds.Contains(userDTO.DefaultBranchId))
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, "Default branch must be one of the assigned branches.");
            }

            try
            {
                var userToAdd = _mapper.Map<AppUser>(userDTO);
                var result = await _userManager.CreateAsync(userToAdd, userDTO.Password);

                if (result.Succeeded)
                {
                    foreach (var branchId in userDTO.BranchIds)
                    {
                        var userBranches = new UserBranches
                        {
                            BranchId = branchId,
                            UserId = userToAdd.Id,
                            IsDefaultBranch = branchId == userDTO.DefaultBranchId
                        };
                        await _unitOfWork.Repository<UserBranches>().AddNewAsync(userBranches);
                    }
                    await _unitOfWork.CommitAsync();
                    var addedUser = await _unitOfWork.Repository<AppUser>().GetByIdAsDtoAsync<GetUserByIdDTO>(userToAdd.Id);

                    return ApiResponseHelper<GetUserByIdDTO>.ResponseSuccess(StatusCodes.CREATED, "New User Created Succeefully.", addedUser);

                }
                string errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.BAD_REQUEST, errorMessages);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<GetUserBranchesDTO>> GetUserBranchesByUserIdAsync(int userId)
        {
            var userBranches = await _userRepository.GetUserBranchesByUserIdAsync(userId);

            return ApiResponseHelper<GetUserBranchesDTO>.ResponseSuccess(data: userBranches);

        }

        public async Task<ApiResponseHelper<GetUserByIdDTO>> EditUserAsync(int Id, EditUserDTO userDTO)
        {
            var oldUser = await _userManager.FindByIdAsync(Id.ToString());
            if (oldUser == null)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"User not found with ID: {Id}");
            }
            try
            {
                _mapper.Map(userDTO, oldUser);
                await _userManager.UpdateAsync(oldUser);

                var updatedUSer = await _unitOfWork.Repository<AppUser>().GetByIdAsDtoAsync<GetUserByIdDTO>(Id);

                return ApiResponseHelper<GetUserByIdDTO>.ResponseSuccess(data: updatedUSer, message: "User updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<GetUserByIdDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<ApiResponseHelper<string>> DeleteUserByIdAsync(int Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"User not found with ID: {Id}");
            }

            try
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    string errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                    return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, errorMessages);
                }
                return ApiResponseHelper<string>.ResponseSuccess(message: $"User {user.UserName} deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<ApiResponseHelper<string>> AssignRolesToUSerAsync(int userId, AssignRolesToUserDTO rolesToUserDTO)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"User not found with ID: {userId}");
            }
            try
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (rolesToUserDTO.RolesIds.Count() == 0)
                {
                    return ApiResponseHelper<string>.ResponseSuccess(message: $"All roles removed from user: {user.UserName}");
                }

                var roleNames = await _roleManager.Roles
                    .Where(r => rolesToUserDTO.RolesIds.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToListAsync();

                if (roleNames.Count() != rolesToUserDTO?.RolesIds.Count())
                {
                    return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, "some role Ids are invalid, pleas double check.");
                }

                var result = await _userManager.AddToRolesAsync(user, roleNames);

                if (!result.Succeeded)
                {
                    string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, errorMessages);
                }
                return ApiResponseHelper<string>.ResponseSuccess(message: "Roles assigned to user succesfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper<string>.ResponseFailure(StatusCodes.BAD_REQUEST, ex.Message);
            }
        }
        public async Task<ApiResponseHelper<object>> ChangeDefaultBranch( int userId, int branchId)
        {
            bool IsBrachExist = await _unitOfWork.Repository<Branch>().IsExistsAsync(b => b.Id == branchId);
            if (!IsBrachExist)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"Branch not found with this ID: {branchId}");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ApiResponseHelper<object>.ResponseFailure(StatusCodes.NOT_FOUND, $"user not found with this ID: {userId}");
            }

            user.DefaultBranchId = branchId;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ApiResponseHelper<object>.ResponseSuccess(StatusCodes.OK, "Default branch changed successfully.");
            }
            return ApiResponseHelper<object>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, "Could not change the default branch.");
        }
    }
}
