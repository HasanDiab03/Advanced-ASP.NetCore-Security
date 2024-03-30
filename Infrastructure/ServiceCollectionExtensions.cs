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
			services.AddScoped<ITokenService, TokenService>();
			return services;
		}
	}
}
