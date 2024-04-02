using Common.Requests.Identity;
using Common.Responses.Wrappers;

namespace Application.Services.Identity
{
	public interface IUserRepository
	{
		Task<IResponseWrapper> RegisterUser(RegisterRequest request);
		Task<IResponseWrapper> GetUserById(string id);
		Task<IResponseWrapper> GetAllUsers();
		Task<IResponseWrapper> UpdateUser(string id, UpdateUserRequest request);
		Task<IResponseWrapper> ChangePassword(string id, ChangePasswordRequest request);
		Task<IResponseWrapper> ChangeUserStatus(string id);
		Task<IResponseWrapper> GetRoles(string id);
		Task<IResponseWrapper> UpdateUserRoles(string id, UpdateUserRolesRequest request);
	}
}
