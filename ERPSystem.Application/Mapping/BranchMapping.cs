using AutoMapper;
using ERPSystem.Application.DTOs.AppUser;
using ERPSystem.Application.DTOs.Branch;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Mapping
{
    public class BranchMapping : Profile
	{
        public BranchMapping()
        {
			CreateMap<Branch, GetBranchDTO>();
			CreateMap<BranchDTO, Branch>();

			CreateMap<UserBranches, GetUserBranchesDTO>()
				.ForMember(dest => dest.DefaultBranchName, opt => opt.MapFrom(src => src.Branch.Name));

			CreateMap<UserBranches, BranchNames>()
				.ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
		}
    }
}
