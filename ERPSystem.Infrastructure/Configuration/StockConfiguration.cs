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


            builder.HasOne(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(b => b.LastUpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
