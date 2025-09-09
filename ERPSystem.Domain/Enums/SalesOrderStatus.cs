using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Enums
{
    public enum SalesOrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Completed = 3,
        Cancelled = 4
    }
}
