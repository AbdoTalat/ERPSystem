using ERPSystem.Application.IRepository;
using ERPSystem.Infrastructure.Repositories;
using ERPSystem.Infrastructure.Seed;
using Helper.Context;
using HotelApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace ERPSystem.API
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

            //App Dependency Injection
            Console.WriteLine(builder.Configuration["JwtSettings:Key"]);
            Console.WriteLine(builder.Configuration["JwtSettings:Key"]);
            builder.Services.AddAppDI(builder.Configuration, new PermissionLoader(builder.Environment));

            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                await seeder.SeedAsync();
            }

            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
			BranchContext.Configure(httpContextAccessor);

			app.UseHttpsRedirection();

			app.UseRouting();
            
            app.UseCors("MyPolicy");

			app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllers();

			app.Run();
		}
	}
}
