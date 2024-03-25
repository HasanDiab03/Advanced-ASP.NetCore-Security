using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig
{
	public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.ToTable("Users", SchemaNames.Secuirty);
		}
	}
	public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.ToTable("Roles", SchemaNames.Secuirty);
		}
	}
	public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
	{
		public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
		{
			builder.ToTable("RoleClaims", SchemaNames.Secuirty);
		}
	}
	public class IdentityUserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
		{
			builder.ToTable("UserClaims", SchemaNames.Secuirty);
		}
	}
	public class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.ToTable("UserRoles", SchemaNames.Secuirty);
		}
	}
	public class IdentityUserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
		{
			builder.ToTable("UserLogins", SchemaNames.Secuirty);
		}
	}
	public class IdentityUserToken : IEntityTypeConfiguration<IdentityUserToken<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
		{
			builder.ToTable("UserTokens", SchemaNames.Secuirty);
		}
	}
}
