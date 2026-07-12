using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.UserContext
{
    public interface IUserContext
    {
        int? UserId { get; }
        Guid? TenantId { get; }
        int? BranchId { get; }
    }
}
