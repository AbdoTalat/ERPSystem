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
    public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            builder.Property(po => po.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(po => po.LineTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(r => r.PurchaseOrder)
              .WithMany(b => b.PurchaseOrderLines)
              .HasForeignKey(r => r.PurchaseOrderId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Product)
              .WithMany(b => b.PurchaseOrderLines)
              .HasForeignKey(r => r.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TenantId);

            builder.HasOne(b => b.Tenant)
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
