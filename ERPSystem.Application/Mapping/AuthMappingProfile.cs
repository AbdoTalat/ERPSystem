using AutoMapper;
using ERPSystem.Application.DTOs.Account;
using ERPSystem.Application.DTOs.AppRole;
using ERPSystem.Application.DTOs.AppUser;
using ERPSystem.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            /* Users Mapping */
            CreateMap<AddUserDTO, AppUser>();
            CreateMap<EditUserDTO, AppUser>();

            CreateMap<AppUser, GetAllUsersDTO>();
            CreateMap<AppUser, GetUserByIdDTO>();

            CreateMap<AppUser, GetUserProfileDTO>();


            /* Roles Mapping */
            CreateMap<AppRole, GetAllRolesDTO>();
            CreateMap<AppRole, GetRoleByIdDTO>();

            CreateMap<RoleDTO, AppRole>();
        }
    }
}
