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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(b => b.LastUpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasIndex(x => x.TenantId);

            builder.HasOne(b => b.Tenant)
                .WithMany(u => u.Users)
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.TenantId, x.NormalizedUserName })
                .IsUnique();

            builder.HasIndex(r => r.NormalizedUserName)
                .IsUnique(false);
        }
    }
}
