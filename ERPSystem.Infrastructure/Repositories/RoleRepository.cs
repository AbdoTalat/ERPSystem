using AutoMapper;
using AutoMapper.QueryableExtensions;
using ERPSystem.Application.DTOs.AppRole;
using ERPSystem.Application.IRepository;
using ERPSystem.Infrastructure.DbContext;
using Helper.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;

        public RoleRepository(AppDbContext context, IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
        }


        public async Task<bool> IsRoleAssignedToAnyUserAsync(int Id)
        {
            var hasUsers = await _context.UserRoles
                        .AnyAsync(ur => ur.RoleId == Id);

            return hasUsers;
        }
        public async Task<List<GetPermissionDTO>> GetAllPermissionsAsync()
        {
            var permissions = await _context.RoleClaims
                .Where(rc => rc.ClaimType == "Permission")
                .Select(rc => rc.ClaimValue)
                .Distinct()
                .ToArrayAsync();

            var grouped = permissions.
                Select(p => new
                {
                    Entity = p.Split('.')[0],
                    Action = p.Split('.')[1]
                })
                .GroupBy(p => p.Entity)
                .Select(p => new GetPermissionDTO
                {
                    Entity = p.Key,
                    Actions = p.Select(x => x.Action).Distinct().ToList()
                })
                .ToList();

            return grouped;
        }
        public async Task<List<string>> GetAllRolesByUserIdAsync(int UserId)
        {
            var roleNames = await (
                from userRoles in _context.UserRoles
                join roles in _context.Roles.IgnoreQueryFilters()
                     on userRoles.RoleId equals roles.Id
                 where userRoles.UserId == UserId
                 select roles.Name
             ).ToListAsync();

            return roleNames;
        }
    }
}
