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
    public class UserBranchesConfiguration : IEntityTypeConfiguration<UserBranches>
    {
        public void Configure(EntityTypeBuilder<UserBranches> builder)
        {
            builder.HasKey(ub => ub.Id);

            builder.HasOne(ub => ub.AppUser)
                .WithMany(u => u.UserBranches)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ub => ub.Branch)
                .WithMany(u => u.UserBranches)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
