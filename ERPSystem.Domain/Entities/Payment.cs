using ERPSystem.Domain.Common;
using ERPSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Payment : BaseEntity, IHasBranch, IHasTenant
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }

        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
