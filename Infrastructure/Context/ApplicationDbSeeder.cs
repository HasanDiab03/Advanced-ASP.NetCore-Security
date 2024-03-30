using Common.Authorization;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
	public class ApplicationDbSeeder
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly ApplicationDbContext _context;

		public ApplicationDbSeeder(UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			ApplicationDbContext context)
        {
            _userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}
		public async Task SeedDatabaseAsync()
		{
			await CheckAndApplyMigrationsAsync();
			await SeedRolesAsync();
			await SeedUserAsync();
		}
		private async Task CheckAndApplyMigrationsAsync()
		{
			if(_context.Database.GetPendingMigrations().Any())
			{
				await _context.Database.MigrateAsync();
			}
		}
		private async Task SeedRolesAsync()
		{
			foreach (var role in AppRoles.DefaultRoles)
			{
				if(!(await _roleManager.RoleExistsAsync(role)))
				{
					var addedRole = new ApplicationRole
					{
						Name = role,
						Description = $"{role} Role"
					};
					await _roleManager.CreateAsync(addedRole);
					// Assing Permission to role
					if(role == AppRoles.Admin) 
					{
						await AssignPermissionsToRoleAsync(addedRole, AppPermissions.AdminPermissions);
					}
					else if (role == AppRoles.Basic)
					{
						await AssignPermissionsToRoleAsync(addedRole, AppPermissions.BasicPermissions);
					}
				}
			}
		}
		private async Task SeedUserAsync()
		{
			var userExists = await _userManager.FindByEmailAsync(AppCredentials.Email);
			if(userExists is null)
			{
				var user = new ApplicationUser
				{
					UserName = AppCredentials.Email.Split("@")[0],
					Email = AppCredentials.Email,
					IsActive = true,
					FirstName = "Junior",
					LastName = "Diab",
					EmailConfirmed = true
				};
				await _userManager.CreateAsync(user, AppCredentials.DefaultPassword);
				await _userManager.AddToRolesAsync(user, AppRoles.DefaultRoles);
			}
		}
		private async Task AssignPermissionsToRoleAsync(ApplicationRole role,
			IReadOnlyList<AppPermission> permissions)
		{	
			var currentClaims = await _roleManager.GetClaimsAsync(role); 
			foreach (var permission in permissions)
			{
				if(!currentClaims.Any(claim => claim.Type == AppClaim.Permission && claim.Value == permission.Name))
				{
					await _context.RoleClaims.AddAsync(new ApplicationRoleClaim
					{
						RoleId = role.Id,
						ClaimType = AppClaim.Permission,
						ClaimValue = permission.Name,
						Description = permission.Description,
						Group = permission.Group,
					});
					await _context.SaveChangesAsync();
				}
			}
		}
    }
}
