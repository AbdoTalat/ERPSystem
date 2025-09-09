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
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.Property(wh => wh.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(wh => wh.Location)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(wh => wh.ContactNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Warehouses)
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