using Application.Services.Identity;
using Infrastructure.Context;
using Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services
			, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddTransient<ApplicationDbSeeder>();
			return services;
		}
		public static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services.AddScoped<ITokenService, TokenService>()
				.AddScoped<IUserRepository, UserRepository>();
			return services;
		}
	}
}
