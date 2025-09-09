using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.DTOs.Branch
{
    public class GetBranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip_Code { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
