using System.Collections.ObjectModel;

namespace Common.Authorization
{
	public record AppPermission(string Feature, string Action, string Group, string Description
		, bool IsBasic = false)
	{
		public string Name => NameFor(Feature, Action);
		public static string NameFor(string feature, string action)
			=> $"Permission.{feature}.{action}";
	}
	public class AppPermissions
	{
		private static readonly AppPermission[] _all = new AppPermission[]
		{
			new(AppFeatures.Users, AppActions.Create, AppRoleGroup.SystemAccess, "Create Users"),
			new(AppFeatures.Users, AppActions.Read, AppRoleGroup.SystemAccess, "Read Users"),
			new(AppFeatures.Users, AppActions.Update, AppRoleGroup.SystemAccess, "Update Users"),
			new(AppFeatures.Users, AppActions.Delete, AppRoleGroup.SystemAccess, "Delete Users"),
			
			new(AppFeatures.UserRoles, AppActions.Read, AppRoleGroup.SystemAccess, "Read User Roles"),
			new(AppFeatures.UserRoles, AppActions.Update, AppRoleGroup.SystemAccess, "Update User Roles"),

			new(AppFeatures.Roles, AppActions.Create, AppRoleGroup.SystemAccess, "Create Roles"),
			new(AppFeatures.Roles, AppActions.Read, AppRoleGroup.SystemAccess, "Read Roles"),
			new(AppFeatures.Roles, AppActions.Update, AppRoleGroup.SystemAccess, "Update Roles"),
			new(AppFeatures.Roles, AppActions.Delete, AppRoleGroup.SystemAccess, "Delete Roles"),

			new(AppFeatures.RoleClaims, AppActions.Read, AppRoleGroup.SystemAccess, "Read Role Claims/Permissions"),
			new(AppFeatures.RoleClaims, AppActions.Update, AppRoleGroup.SystemAccess, "Update Role Claims/Permissions"),

			new(AppFeatures.Employees, AppActions.Create, AppRoleGroup.ManagementHierarchy, "Create Employees"),
			new(AppFeatures.Employees, AppActions.Read, AppRoleGroup.ManagementHierarchy, "Read Employees", true),
			new(AppFeatures.Employees, AppActions.Update, AppRoleGroup.ManagementHierarchy, "Update Employees"),
			new(AppFeatures.Employees, AppActions.Delete, AppRoleGroup.ManagementHierarchy, "Delete Employees"),

		};
		public static IReadOnlyList<AppPermission> AdminPermissions { get; }
			= new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsBasic).ToArray());
		public static IReadOnlyList<AppPermission> BasicPermissions { get; }
			= new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());
		public static IReadOnlyList<AppPermission> AllPermission { get; } =
			new ReadOnlyCollection<AppPermission>(_all);
	}
}
