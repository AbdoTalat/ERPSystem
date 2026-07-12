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

            builder.HasIndex(x => x.TenantId);

            builder.HasOne(b => b.Tenant)
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            AuditableEntityConfiguration.Configure(builder);
        }
    }
}
