using API.Permissions;
using Application.AppConfigs;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace API
{
	public static class Extensions
	{
		public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			var seeder = scope.ServiceProvider.GetService<ApplicationDbSeeder>();
			seeder.SeedDatabaseAsync().GetAwaiter().GetResult(); // to wait for the seeding to finish
			return app;
		}
		public static IServiceCollection AddIdentitySettings(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
					ValidateIssuer = false,
					ValidateAudience = false,
					RoleClaimType = ClaimTypes.Role,
					ClockSkew = TimeSpan.Zero
				}
			});
			services
				.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
				.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
				.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
			{
				opt.Password.RequiredLength = 6;
				opt.Password.RequireDigit = false;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
				opt.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>();
			return services;
		}
		public static IServiceCollection AddMyOptions(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions<AppConfiguration>()
				.Bind(configuration.GetSection("JWT"))
				.ValidateDataAnnotations();
			return services;
		}
	}
}
