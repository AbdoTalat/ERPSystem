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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Description)
                .HasMaxLength(200);

            builder.Property(b => b.Street)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.City)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.ContactNumber)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(b => b.Zip_Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.TenantId);

            builder.HasOne(b => b.Tenant)
                .WithMany(b => b.Branches)
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            AuditableEntityConfiguration.Configure(builder);
        }
    }
}
