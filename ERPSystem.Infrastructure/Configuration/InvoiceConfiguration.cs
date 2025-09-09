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
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
               .HasConversion<string>()
               .HasMaxLength(20);

            builder.HasOne(i => i.SalesOrder)
                .WithMany(so => so.Invoices)
                .HasForeignKey(i => i.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Branch)
                .WithMany(b => b.Invoices)
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(i => i.LastUpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
