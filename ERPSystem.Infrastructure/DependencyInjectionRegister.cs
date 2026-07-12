using ERPSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ERPSystem.Domain;
using ERPSystem.Infrastructure.Repositories;
using ERPSystem.Domain.Entities.Auth;
using ERPSystem.Infrastructure.Seed;
using HotelApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using ERPSystem.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using ERPSystem.Infrastructure.UnitOfWorks;
using ERPSystem.Application.IRepository;


namespace ERPSystem.Infrastructure
{
    public static class DependencyInjectionRegister
	{
		public static IServiceCollection AddInfrastructureDI(this IServiceCollection services,
			IConfiguration configuration, IPermissionLoader permissionLoader)
		{
			services.AddScoped<IUnitOfWork, unitOfWork>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IBranchRepository, BranchRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
			services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();

			services.AddHttpContextAccessor();


			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnString"));
			});

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddIdentity<AppUser, AppRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders() 
				.AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>(); 

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();


            var allPermissions = permissionLoader.LoadAllPermissions();

            services.AddAuthorization(options =>
            {
                foreach (var permission in allPermissions)
                {
                    options.AddPolicy(permission, policy =>
                        policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });
            return services;
		}
	}
}
