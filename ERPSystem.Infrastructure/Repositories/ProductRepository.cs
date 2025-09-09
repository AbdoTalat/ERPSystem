using ERPSystem.Application.DTOs.SalesOrder;
using ERPSystem.Application.IRepository;
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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<int>> CheckProductsExistById(List<int> ProductIDs)
        {
            var existingProducts = await _context.Products
                .Where(p => ProductIDs.Contains(p.Id))
                .BranchFilter()
                .ToListAsync();

            var existingIds = existingProducts.Select(p => p.Id).ToHashSet();

            var invalidProductIds = ProductIDs.Where(id => !existingIds.Contains(id)).ToList();

            return invalidProductIds;
        }

        //public async Task<List<int>> CheckProductQuantity(Dictionary<int, int> products)
        //{
        //    var 
        //}
    }
}
