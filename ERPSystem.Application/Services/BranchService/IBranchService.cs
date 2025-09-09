using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.API;
using ERPSystem.Application.DTOs.Branch;

namespace ERPSystem.Application.Services.BranchManagement
{
    public interface IBranchService
	{
		Task<int> GetDefaultBranchIdByUserIdAsync(int userId);

		Task<ApiResponseHelper<IEnumerable<GetBranchDTO>>> GetAllBranchesAsync();
		Task<ApiResponseHelper<GetBranchDTO>> GetBranchByIdAsync(int id);
		Task<ApiResponseHelper<GetBranchDTO>> AddBranchAsync(BranchDTO branchDTO);
		Task<ApiResponseHelper<GetBranchDTO>> UpdateBranchAsync(BranchDTO branchDTO, int id);
		Task<ApiResponseHelper<string>> DeleteBranchAsync(int id);
	}
}
