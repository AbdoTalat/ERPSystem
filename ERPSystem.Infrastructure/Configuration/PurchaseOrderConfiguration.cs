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
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.Property(po => po.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(po => po.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(r => r.Branch)
              .WithMany(b => b.PurchaseOrders)
              .HasForeignKey(r => r.BranchId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Supplier)
              .WithMany(b => b.PurchaseOrders)
              .HasForeignKey(r => r.SupplierId)
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
