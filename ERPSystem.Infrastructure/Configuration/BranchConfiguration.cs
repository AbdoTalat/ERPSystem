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
