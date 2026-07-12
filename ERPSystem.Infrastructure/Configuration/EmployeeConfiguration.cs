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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Salary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Gender)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(e => e.HoursWorked)
                .IsRequired();

            builder.Property(e => e.Salary)
                .IsRequired();

            builder.Property(e => e.JobTitle)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Employees)
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
