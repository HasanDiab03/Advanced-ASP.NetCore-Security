using API.Attributes;
using Application.Features.Identity.Commands;
using Application.Features.Identity.Queries;
using Common.Authorization;
using Common.Requests.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{
	[Route("api/[controller]")]
	public class RolesController : BaseController<RolesController>
	{
		[HttpPost]
		[MustHavePermission(AppFeatures.Roles, AppActions.Create)]
		public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
			=> HandleResult(await Mediator.Send(new CreateRoleCommand(request)));
		[HttpGet]
		[MustHavePermission(AppFeatures.Roles, AppActions.Read)]
		public async Task<IActionResult> GetAllRoles()
			=> HandleResult(await Mediator.Send(new GetAllAppRolesQuery()));
		[HttpPut("{id}")]
		[MustHavePermission(AppFeatures.Roles, AppActions.Update)]
		public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateRoleRequest request)
			=> HandleResult(await Mediator.Send(new UpdateRoleCommand(id, request)));
		[HttpGet("{id}")]
		[MustHavePermission(AppFeatures.Roles, AppActions.Read)]
		public async Task<IActionResult> GetRole([FromRoute] string id)
			=> HandleResult(await Mediator.Send(new GetRoleByIdQuery(id)));
		[HttpDelete("{id}")]
		[MustHavePermission(AppFeatures.Roles, AppActions.Delete)]
		public async Task<IActionResult> DeleteRole([FromRoute] string id)
			=> HandleResult(await Mediator.Send(new DeleteRoleCommand(id)));
		[HttpGet("permission/{id}")]
		[MustHavePermission(AppFeatures.RoleClaims, AppActions.Read)]
		public async Task<IActionResult> GetPermissions([FromRoute] string id)
			=> HandleResult(await Mediator.Send(new GetClaimsQuery(id)));
		[HttpPut("permissions/{id}")]
		public async Task<IActionResult> UpdateRolePermissions([FromRoute] string id
			, [FromBody] UpdateRolePermissionsRequest request)
			=> HandleResult(await Mediator.Send(new UpdateRolePermissionsCommand(id, request)));
	}
}
