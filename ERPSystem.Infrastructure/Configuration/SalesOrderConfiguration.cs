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
    public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.Property(so => so.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(so => so.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(so => so.Customer)
                .WithMany(c => c.SalesOrders)
                .HasForeignKey(so => so.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(so => so.Branch)
                .WithMany(b => b.SalesOrders)
                .HasForeignKey(so => so.BranchId)
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
