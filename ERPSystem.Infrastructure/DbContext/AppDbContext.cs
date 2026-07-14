using ERPSystem.Application.Services.UserContext;
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
    public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
	{
        private readonly IUserContext _userContext;
        public AppDbContext(DbContextOptions options, IUserContext userContext) 
            : base(options)
        {
            _userContext = userContext;
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

        public DbSet<Tenant> Tenants { get; set; }

        #endregion

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            #region Query Filters For Multi-Tenancy

            builder.Entity<Branch>().HasQueryFilter(b => b.TenantId == _userContext.TenantId);
            builder.Entity<Employee>().HasQueryFilter(e => e.TenantId == _userContext.TenantId);
            builder.Entity<Department>().HasQueryFilter(d => d.TenantId == _userContext.TenantId);
            builder.Entity<UserBranches>().HasQueryFilter(ub => ub.TenantId == _userContext.TenantId);
            builder.Entity<Category>().HasQueryFilter(c => c.TenantId == _userContext.TenantId);
            builder.Entity<Product>().HasQueryFilter(p => p.TenantId == _userContext.TenantId);
            builder.Entity<Stock>().HasQueryFilter(s => s.TenantId == _userContext.TenantId);
            builder.Entity<StockMovement>().HasQueryFilter(sm => sm.TenantId == _userContext.TenantId);
            builder.Entity<Warehouse>().HasQueryFilter(w => w.TenantId == _userContext.TenantId);
            builder.Entity<Supplier>().HasQueryFilter(s => s.TenantId == _userContext.TenantId);
            builder.Entity<PurchaseOrder>().HasQueryFilter(po => po.TenantId == _userContext.TenantId);
            builder.Entity<GoodsReceipt>().HasQueryFilter(gr => gr.TenantId == _userContext.TenantId);
            builder.Entity<Customer>().HasQueryFilter(c => c.TenantId == _userContext.TenantId);
            builder.Entity<SalesOrder>().HasQueryFilter(so => so.TenantId == _userContext.TenantId);
            builder.Entity<Invoice>().HasQueryFilter(i => i.TenantId == _userContext.TenantId);
            builder.Entity<Payment>().HasQueryFilter(p => p.TenantId == _userContext.TenantId);
            builder.Entity<Tenant>().HasQueryFilter(t => t.Id == _userContext.TenantId);
            builder.Entity<AppUser>().HasQueryFilter(u => u.TenantId == _userContext.TenantId);
            builder.Entity<AppRole>().HasQueryFilter(r => r.TenantId == _userContext.TenantId);

            #endregion

            #region Configurations

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

            /* Tenant Configuration*/
            builder.ApplyConfiguration(new TenantConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

            #endregion
        }
    }
}
