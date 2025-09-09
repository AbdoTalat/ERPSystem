using ERPSystem.Domain.Common;
using ERPSystem.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip_Code { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }


        public ICollection<UserBranches> UserBranches { get; set; } = new HashSet<UserBranches>();
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
        public ICollection<Department> Departments { get; set; } = new HashSet<Department>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public ICollection<Stock> Stocks { get; set; } = new HashSet<Stock>();
        public ICollection<Warehouse> Warehouses { get; set; } = new HashSet<Warehouse>();
        public ICollection<StockMovement> StockMovements { get; set; } = new HashSet<StockMovement>();
        public ICollection<Supplier> Suppliers { get; set; } = new HashSet<Supplier>();
        public ICollection<PurchaseOrder> PurchaseOrders{ get; set; } = new HashSet<PurchaseOrder>();
        public ICollection<GoodsReceipt> GoodsReceipts  { get; set; } = new HashSet<GoodsReceipt>();
        public ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
        public ICollection<SalesOrder> SalesOrders { get; set; } = new HashSet<SalesOrder>();
        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
