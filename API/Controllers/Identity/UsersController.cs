using API.Attributes;
using Application.Features.Identity.Commands;
using Application.Features.Identity.Queries;
using Common.Authorization;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.Controllers.Identity
{
	[Route("api/[controller]")]
	public class UsersController : BaseController<UsersController>
	{
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest command)
		{
			var result = await Mediator.Send(new RegisterUserCommand(command));
			return HandleResult(result);
		}
		[HttpGet("{id}")]
		[MustHavePermission(AppFeatures.Users, AppActions.Read)]
		public async Task<IActionResult> GetUser([FromRoute] string id)
		{
			var result = await Mediator.Send(new GetUserQuery(id));
			return HandleResult(result);
		}
		[HttpGet]
		[MustHavePermission(AppFeatures.Users, AppActions.Read)]
		public async Task<IActionResult> GetUsers()
		{
			var result = await Mediator.Send(new GetUsersQuery());
			return HandleResult(result);
		}
		[HttpPut("{id}")]
		[MustHavePermission(AppFeatures.Users, AppActions.Update)]
		public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserRequest request)
		{
			var result = await Mediator.Send(new UpdateUserCommand(id, request));
			return HandleResult(result);
		}
		[HttpPut("change-password/{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordRequest request)
		{
			var result = await Mediator.Send(new ChangePasswordCommand(id, request));
			return HandleResult(result);
		}
		[HttpPut("change-status/{id}")]
		[MustHavePermission(AppFeatures.Users, AppActions.Update)]
		public async Task<IActionResult> ChangeStatus([FromRoute] string id)
			=> HandleResult(await Mediator.Send(new ChangeUserStatusCommand(id)));
		[HttpGet("roles/{id}")]
		[MustHavePermission(AppFeatures.Roles, AppActions.Read)]
		public async Task<IActionResult> GetRoles([FromRoute] string id)
			=> HandleResult(await Mediator.Send(new GetRolesQuery(id)));
		[HttpPut("update-roles/{id}")]
		[MustHavePermission(AppFeatures.Users, AppActions.Update)]
		public async Task<IActionResult> UpdateUserRoles([FromRoute] string id, [FromBody] UpdateUserRolesRequest request)
			=> HandleResult(await Mediator.Send(new UpateUserRolesCommand(id, request)));
	}
}
