using Common.Responses.Identity;

namespace Common.Requests.Identity
{
	public class UpdateUserRolesRequest
	{
		public List<UserRoleViewModel> Roles { get; set; }
	}
}
