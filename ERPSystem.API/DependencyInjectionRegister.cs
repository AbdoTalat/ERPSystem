using ERPSystem.Application;
using ERPSystem.Application.IRepository;
using ERPSystem.Domain;
using ERPSystem.Infrastructure;

namespace ERPSystem.API
{
	public static class DependencyInjectionRegister
	{
		public static IServiceCollection AddAppDI(this IServiceCollection services,
			IConfiguration configuration, IPermissionLoader permissionLoader)
		{
			services.AddInfrastructureDI(configuration, permissionLoader)
				.AddApplicationDI(configuration);

			return services;
		}
	}
}
