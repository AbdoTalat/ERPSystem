using ERPSystem.Application.IRepository;
using ERPSystem.Domain.Entities;
using ERPSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly AppDbContext _context;

        public PurchaseOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderForReceivingGoodsAsync(int purchaseOrderId)
        {
            var po = await _context.PurchaseOrders
                 .Include(po => po.PurchaseOrderLines)
                    .Include(p => p.GoodsReceipts)
                        .ThenInclude(gr => gr.GoodsReceiptLines)
                         .FirstOrDefaultAsync(po => po.Id == purchaseOrderId);

            return po;
        }
    }
}
