using ERPSystem.Application.DTOs.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.IRepository
{
    public interface IUserRepository
    {
		Task<GetUserBranchesDTO> GetUserBranchesByUserIdAsync(int userId);
	}
}
