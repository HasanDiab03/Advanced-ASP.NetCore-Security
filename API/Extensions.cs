using Infrastructure.Context;
using Infrastructure.Models;

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
		public static IServiceCollection AddIdentitySettings(this IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
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
	}
}
