using ERPSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Configuration
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.Property(s => s.AvailableQuantity)
                .HasComputedColumnSql("[Quantity] - ([ReservedQuantity] + [DamagedQuantity])", stored: true);

            builder.HasOne(s => s.Product)
                   .WithMany(p => p.Stocks)
                   .HasForeignKey(s => s.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Warehouse)
                   .WithMany(w => w.Stocks)
                   .HasForeignKey(s => s.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Stocks)
                .HasForeignKey(r => r.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TenantId);

            builder.HasOne(b => b.Tenant)
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            AuditableEntityConfiguration.Configure(builder);
        }
    }
}
