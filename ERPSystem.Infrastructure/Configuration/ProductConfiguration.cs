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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.Barcode)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.SKU)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Products)
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
