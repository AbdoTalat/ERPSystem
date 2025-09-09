using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities.Auth
{
    public class UserBranches
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public bool IsDefaultBranch { get; set; }
    }
}
