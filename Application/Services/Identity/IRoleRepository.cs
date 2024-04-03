using Common.Requests.Identity;
using Common.Responses.Wrappers;

namespace Application.Services.Identity
{
	public interface IRoleRepository
	{
		Task<IResponseWrapper> CreateRole(CreateRoleRequest request);
		Task<IResponseWrapper> GetAllRoles();
		Task<IResponseWrapper> UpdateRole(string id, UpdateRoleRequest request);
		Task<IResponseWrapper> GetRole(string id);
		Task<IResponseWrapper> DeleteRole(string id);
		Task<IResponseWrapper> GetPermissions(string id);
		Task<IResponseWrapper> UpdateRolePermissions(string id, UpdateRolePermissionsRequest request);
	}
}
