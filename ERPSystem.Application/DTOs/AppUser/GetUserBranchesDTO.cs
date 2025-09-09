using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.AppUser
{
    public class GetUserBranchesDTO
    {
        public string DefaultBranchName { get; set; }
        public IEnumerable<BranchNames> BranchNames { get; set; }
    }
    public class BranchNames
    {
        public string BranchName { get; set; }
    }
}
