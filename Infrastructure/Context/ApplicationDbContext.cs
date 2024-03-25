using Domain;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, 
		string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, 
		ApplicationRoleClaim, IdentityUserToken<string>>
	{
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
		public DbSet<Employee> Employees { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			foreach (var property in builder.Model.GetEntityTypes()
				.SelectMany(t => t.GetProperties())
				.Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
			{
				property.SetColumnType("decimal(18,2)");
			} 
			// this basically just iterates over all entities, get their Decimal properties, and then sets them to decimal(18,2) in the db
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
			// this will make it scan the whole assembly (infrastructure project) and look for classes that inherit IEntityTypeConfiguration
		}
	}
}
