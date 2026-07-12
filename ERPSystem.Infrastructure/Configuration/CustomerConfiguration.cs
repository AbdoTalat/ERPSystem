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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(s => s.Email)
               .HasMaxLength(100);

            builder.Property(s => s.Phone)
               .HasMaxLength(20);

            builder.Property(s => s.Address)
               .HasMaxLength(100);

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Customers)
                .HasForeignKey(r => r.BranchId)
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
