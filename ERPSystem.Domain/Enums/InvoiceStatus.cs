using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Enums
{
    public enum InvoiceStatus
    {
        Unpaid = 0,
        PartiallyPaid = 1,
        Paid = 2,
        Cancelled = 3
    }
}
