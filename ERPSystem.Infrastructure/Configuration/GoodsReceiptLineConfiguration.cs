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
    public class GoodsReceiptLineConfiguration : IEntityTypeConfiguration<GoodsReceiptLine>
    {
        public void Configure(EntityTypeBuilder<GoodsReceiptLine> builder)
        {
            builder.HasOne(r => r.GoodsReceipt)
              .WithMany(b => b.GoodsReceiptLines)
              .HasForeignKey(r => r.GoodsReceiptId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Product)
              .WithMany(b => b.GoodsReceiptLines)
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
