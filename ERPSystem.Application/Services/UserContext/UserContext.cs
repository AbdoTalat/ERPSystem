using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.Services.UserContext
{
    /// <summary>
    /// This Context for the current user, it provides access to the user's claims and other relevant information.
    /// </summary>
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public int? UserId =>
            int.TryParse(_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var UserId)
            ? UserId : null;

        public Guid? TenantId =>
            Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantId"), out var tenantId)
            ? tenantId : null;

        public int? BranchId =>
            int.TryParse(_httpContextAccessor?.HttpContext?.User?.FindFirst("DefaultBranchId")?.Value, out var BranchId)
            ? BranchId : null;
    }
}
