using ERPSystem.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Configuration
{
    public static class AuditableEntityConfiguration
    {
        public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity :  AuditableEntity
        {
            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.LastUpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.LastUpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
