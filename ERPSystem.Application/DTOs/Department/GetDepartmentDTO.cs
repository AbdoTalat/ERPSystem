using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Application.DTOs.Employee;

namespace ERPSystem.Application.DTOs.Department
{
    public class GetDepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ManagerName { get; set; }
    }
}
