using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Enums
{
    public enum PurchaseOrderStatus
    {
        Pending = 0,
        Approved = 1,
        FullyReceived = 2,
        PartiallyReceived = 3,
        Cancelled = 4
    }
}
