using ERPSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Warehouse : BaseEntity, IHasBranch
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string ContactNumber { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new HashSet<Stock>();
        public ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new HashSet<SalesOrderLine>();
    }
}
