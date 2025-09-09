using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Configuration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
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
               .WithMany(b => b.Suppliers)
               .HasForeignKey(r => r.BranchId)
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
