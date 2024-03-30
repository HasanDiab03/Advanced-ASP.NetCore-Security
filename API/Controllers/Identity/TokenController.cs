using Application.Features.Identity.Queries;
using Azure.Core;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{
	[Route("api/[controller]")]
	public class TokenController : BaseController<TokenController>
	{
		[HttpPost("get-token")]
		[AllowAnonymous]
		public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest request)
		{ 
			var response = await Mediator.Send(new GetTokenQuery(request));
			if(response.IsSuccessful)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		[HttpPost("refresh-token")]
		public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest request)
		{
			var response = await Mediator.Send(new GetRefreshTokenQuery(request));
			if (response.IsSuccessful)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
	}
}
