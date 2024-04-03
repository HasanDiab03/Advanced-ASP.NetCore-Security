using Common.Responses.Identity;

namespace Common.Requests.Identity
{
	public class UpdateRolePermissionsRequest
	{
		public List<RoleClaimViewModel> RoleClaims { get; set; }
	}
}
