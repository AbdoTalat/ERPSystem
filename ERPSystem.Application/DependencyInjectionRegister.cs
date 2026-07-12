using ERPSystem.Application.Services.AccountService;
using ERPSystem.Application.Services.BranchManagement;
using ERPSystem.Application.Services.CategoryService;
using ERPSystem.Application.Services.CustomerService;
using ERPSystem.Application.Services.DepartmentService;
using ERPSystem.Application.Services.EmployeeService;
using ERPSystem.Application.Services.GoodsReceiptService;
using ERPSystem.Application.Services.InvoiceService;
using ERPSystem.Application.Services.PaymentService;
using ERPSystem.Application.Services.ProductService;
using ERPSystem.Application.Services.PurchaseOrderService;
using ERPSystem.Application.Services.RoleService;
using ERPSystem.Application.Services.SalesOrderService;
using ERPSystem.Application.Services.StockService;
using ERPSystem.Application.Services.SupplierService;
using ERPSystem.Application.Services.TokenService;
using ERPSystem.Application.Services.UserContext;
using ERPSystem.Application.Services.UserService;
using ERPSystem.Application.Services.WarehouseService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ERPSystem.Application
{
    public static class DependencyInjectionRegister 
	{
		public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IDepartmentService, DepartmentService>();
			services.AddScoped<IEmployeeService, EmployeeService>();

			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IGoodsReceiptService, GoodsReceiptService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISalesOrderService, SalesOrderService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUserContext, UserContext>();

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper((Action<AutoMapper.IMapperConfigurationExpression>?)null, AppDomain.CurrentDomain.GetAssemblies());


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });

            services.AddAuthorization();


            return services;
		}
	}
}
