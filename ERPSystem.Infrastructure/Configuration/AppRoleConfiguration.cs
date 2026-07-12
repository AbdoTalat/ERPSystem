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
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
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
                .WithMany()
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.TenantId, x.NormalizedName })
                .IsUnique();

            builder.HasIndex(r => r.NormalizedName)
                .IsUnique(false);
        }
    }
}
