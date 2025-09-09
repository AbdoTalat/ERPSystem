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
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.Property(gr => gr.Notes)
                .HasMaxLength(300);

            builder.HasOne(r => r.Branch)
              .WithMany(b => b.GoodsReceipts)
              .HasForeignKey(r => r.BranchId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.PurchaseOrder)
              .WithMany(b => b.GoodsReceipts)
              .HasForeignKey(r => r.PurchaseOrderId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ReceivedBy)
              .WithMany(b => b.GoodsReceipts)
              .HasForeignKey(r => r.ReceivedById)
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
