using ERPSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Configuration
{
    public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
    {
        public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
        {
            builder.Property(sol => sol.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(sol => sol.LineTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(sol => sol.SalesOrder)
                .WithMany(so => so.SalesOrderLines)
                .HasForeignKey(sol => sol.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sol => sol.Product)
                .WithMany(p => p.SalesOrderLines)
                .HasForeignKey(sol => sol.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sol => sol.Warehouse)
               .WithMany(p => p.SalesOrderLines)
               .HasForeignKey(sol => sol.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
