using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.AppRole
{
    public class GetPermissionDTO
    {
        public string Entity { get; set; }
        public List<string> Actions { get; set; } = new List<string>();
    }
}
