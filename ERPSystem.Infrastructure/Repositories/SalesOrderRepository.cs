using ERPSystem.Application.IRepository;
using ERPSystem.Domain.Entities;
using ERPSystem.Infrastructure.DbContext;
using Helper.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly AppDbContext _context;

        public SalesOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SalesOrder?> GetSalesOrderWithSalesOrderLineAsync(int Id)
        {
            return await _context.SalesOrders
                .BranchFilter()
                .Include(so => so.SalesOrderLines)
                .FirstOrDefaultAsync(so => so.Id == Id);
        }
    }
}
