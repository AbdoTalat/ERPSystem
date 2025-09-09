using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.AppUser
{
    public class AssignRolesToUserDTO
    {
        public List<int> RolesIds { get; set; } = new List<int>();
    }
}
