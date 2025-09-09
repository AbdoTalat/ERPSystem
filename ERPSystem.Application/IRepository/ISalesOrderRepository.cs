using ERPSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Application.IRepository
{
    public interface ISalesOrderRepository
    {
        Task<SalesOrder?> GetSalesOrderWithSalesOrderLineAsync(int Id);
    }
}
