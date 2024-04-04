using API.Attributes;
using API.Controllers.Identity;
using Application.Features.Employees.Commands;
using Application.Features.Employees.Queries;
using Common.Authorization;
using Common.Requests;
using Common.Responses.Wrappers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	public class EmployeesController : BaseController<EmployeesController>
	{
		[HttpPost]
		[MustHavePermission(AppFeatures.Employees, AppActions.Create)]
		public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeRequest request)
		{
			var result = await Mediator.Send(new CreateEmployeeCommand(request));
			if(!result.IsSuccessful)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[HttpPut("{id}")]
		[MustHavePermission(AppFeatures.Employees, AppActions.Update)]
		public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] UpdateEmployeeRequest request)
		{
			var result = await Mediator.Send(new UpdateEmployeeCommand(id, request));
			if(!result.IsSuccessful)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[HttpDelete("{id}")]
		[MustHavePermission(AppFeatures.Employees, AppActions.Delete)]
		public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
		{
			var result = await Mediator.Send(new DeleteEmployeeCommand(id));
			if(!result.IsSuccessful)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetEmployees()
		{
			var result = await Mediator.Send(new GetEmployeesQuery());
			if(!result.IsSuccessful)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[HttpGet("{id}")]
		[MustHavePermission(AppFeatures.Employees, AppActions.Read)]
		public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
		{
			var result = await Mediator.Send(new GetEmployeeQuery(id));
			if(!result.IsSuccessful)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
