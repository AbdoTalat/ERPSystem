using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Application.IRepository;
using Helper.API;
using Helper.Constants;
using AutoMapper;
using ERPSystem.Domain.Entities;
using ERPSystem.Application.DTOs.Branch;

namespace ERPSystem.Application.Services.BranchManagement
{
    public class BranchService : IBranchService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IBranchRepository _branchRepository;

		public BranchService(IUnitOfWork unitOfWork,IMapper mapper, IBranchRepository branchRepository)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_branchRepository = branchRepository;
		}
		public async Task<int> GetDefaultBranchIdByUserIdAsync(int userId)
		{
			var DefualtBranchId = await _branchRepository.GetDefaultBranchIdByUserIdAsync(userId);

			return DefualtBranchId;
		}

		public async Task<ApiResponseHelper<IEnumerable<GetBranchDTO>>> GetAllBranchesAsync()
		{
			var branches = await _unitOfWork.Repository<Branch>().GetAllAsDtoAsync<GetBranchDTO>();
			return ApiResponseHelper<IEnumerable<GetBranchDTO>>.ResponseSuccess(data: branches);
		}

		public async Task<ApiResponseHelper<GetBranchDTO>> GetBranchByIdAsync(int id)
		{
			var branch = await _unitOfWork.Repository<Branch>().GetByIdAsDtoAsync<GetBranchDTO>(id);
			if (branch == null)
			{
				return ApiResponseHelper<GetBranchDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Branch not found with ID: {id}");
			}

			return ApiResponseHelper<GetBranchDTO>.ResponseSuccess(data: branch);
		}

		public async Task<ApiResponseHelper<GetBranchDTO>> AddBranchAsync(BranchDTO branchDTO)
		{
			try
			{
				var branch = _mapper.Map<Branch>(branchDTO);
				await _unitOfWork.Repository<Branch>().AddNewAsync(branch);
				await _unitOfWork.CommitAsync();

				var addedBranch = await _unitOfWork.Repository<Branch>().GetByIdAsDtoAsync<GetBranchDTO>(branch.Id);

				return ApiResponseHelper<GetBranchDTO>.ResponseSuccess(StatusCodes.CREATED, "Branch created successfully.", addedBranch);
			}
			catch (Exception ex)
			{
				return ApiResponseHelper<GetBranchDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
			}
		}

		public async Task<ApiResponseHelper<GetBranchDTO>> UpdateBranchAsync(BranchDTO branchDTO, int id)
		{
			var existingBranch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
			if (existingBranch == null)
			{
				return ApiResponseHelper<GetBranchDTO>.ResponseFailure(StatusCodes.NOT_FOUND, $"Branch not found with ID: {id}");
			}

			try
			{
				_mapper.Map(branchDTO, existingBranch);
				_unitOfWork.Repository<Branch>().Update(existingBranch);
				await _unitOfWork.CommitAsync();

				var updatedBranch = await _unitOfWork.Repository<Branch>().GetByIdAsDtoAsync<GetBranchDTO>(id);

				return ApiResponseHelper<GetBranchDTO>.ResponseSuccess(message: "Branch updated successfully.", data: updatedBranch);
			}
			catch (Exception ex)
			{
				return ApiResponseHelper<GetBranchDTO>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
			}
		}

		public async Task<ApiResponseHelper<string>> DeleteBranchAsync(int id)
		{
			var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
			if (branch == null)
			{
				return ApiResponseHelper<string>.ResponseFailure(StatusCodes.NOT_FOUND, $"Branch not found with ID: {id}");
			}

			try
			{
				_unitOfWork.Repository<Branch>().Delete(branch);
				await _unitOfWork.CommitAsync();

				return ApiResponseHelper<string>.ResponseSuccess(message: $"Branch '{branch.Name}' deleted successfully.");
			}
			catch (Exception ex)
			{
				return ApiResponseHelper<string>.ResponseFailure(StatusCodes.INTERNAL_SERVER_ERROR, ex.Message);
			}
		}
	}
}
