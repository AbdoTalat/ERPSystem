using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPSystem.Domain.Common;

namespace ERPSystem.Domain.Entities
{
    public class Category : BaseEntity, IHasBranch
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
