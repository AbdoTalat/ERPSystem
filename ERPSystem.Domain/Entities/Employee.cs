using ERPSystem.Domain.Common;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Employee : BaseEntity, IHasBranch
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public int HoursWorked { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public string JobTitle { get; set; }
        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new HashSet<GoodsReceipt>();
    }
}
