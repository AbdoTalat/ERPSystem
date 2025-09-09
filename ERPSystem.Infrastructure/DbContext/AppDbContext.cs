using ERPSystem.Domain.Common;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.DbContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
	{
        private readonly IHttpContextAccessor _httpContextAccessor;

        //public AppDbContext() { }
        public AppDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) 
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

		#region DbSets
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
        
		public DbSet<UserBranches> UserBranches { get; set; }
		public DbSet<Branch> Branches { get; set; }

		public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
		public DbSet<Stock> Stocks { get; set; }
		public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
		
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
		public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
		public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
		public DbSet<GoodsReceiptLine> GoodsReceiptLines { get; set; }

		public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        #endregion

        #region Overrided SaveChangesAsync Method
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await SaveChangesAsync(skipAuditFields: false, cancellationToken);
		}

		public async Task<int> SaveChangesAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default)
		{
			if (!skipAuditFields)
			{
				var entries = ChangeTracker
					.Entries<BaseEntity>()
					.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

				var now = DateTime.UtcNow;

				// Get userId safely
				var userIdStr = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				int? userId = null;
				if (int.TryParse(userIdStr, out var parsedUserId))
					userId = parsedUserId;

				foreach (var entry in entries)
				{
					if (entry.State == EntityState.Added)
					{
						entry.Entity.CreatedDate = now;
						if (userId.HasValue)
							entry.Entity.CreatedById = userId.Value;
					}
					else if (entry.State == EntityState.Modified)
					{
						entry.Entity.LastUpdatedDate = now;
						if (userId.HasValue)
							entry.Entity.LastUpdatedById = userId.Value;
					}
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
		#endregion


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			/* Auth Configurations */
			builder.ApplyConfiguration(new AppUserConfiguration());
			builder.ApplyConfiguration(new AppRoleConfiguration());

			/* HR Configurations */
			builder.ApplyConfiguration(new EmployeeConfiguration());
			builder.ApplyConfiguration(new DepartmentConfiguration());

            /* Invetory Configurations */
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new StockConfiguration());
            builder.ApplyConfiguration(new WarehouseConfiguration());
			builder.ApplyConfiguration(new StockMovementConfiguration());

			/* Branch Management Configurations */
			builder.ApplyConfiguration(new UserBranchesConfiguration());
			builder.ApplyConfiguration(new BranchConfiguration());

            /* Purchasing Configurations */
            builder.ApplyConfiguration(new SupplierConfiguration());
            builder.ApplyConfiguration(new PurchaseOrderConfiguration());
            builder.ApplyConfiguration(new PurchaseOrderLineConfiguration());
            builder.ApplyConfiguration(new GoodsReceiptConfiguration());
            builder.ApplyConfiguration(new GoodsReceiptLineConfiguration());

            /* Sales Configuration*/
			builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new SalesOrderConfiguration());
            builder.ApplyConfiguration(new SalesOrderLineConfiguration());
            builder.ApplyConfiguration(new InvoiceConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
        }
    }
}
