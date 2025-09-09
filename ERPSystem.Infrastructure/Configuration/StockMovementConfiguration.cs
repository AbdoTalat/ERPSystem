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
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Reason)
                .HasMaxLength(200);

            builder.HasOne(sm => sm.Stock)
                .WithMany(s => s.StockMovements)
                .HasForeignKey(sm => sm.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.Warehouse)
                .WithMany()
                .HasForeignKey(sm => sm.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.Branch)
                .WithMany(b => b.StockMovements)
                .HasForeignKey(sm => sm.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.Product)
               .WithMany()
               .HasForeignKey(sm => sm.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.AppUser)
                .WithMany()
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
