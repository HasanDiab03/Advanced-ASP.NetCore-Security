using Common.Responses.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Identity
{
	[ApiController]
	public class BaseController<T> : ControllerBase
	{
		private IMediator _mediator;
        protected IMediator Mediator  => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
		protected IActionResult HandleResult(IResponseWrapper result)
			=> result.IsSuccessful ? Ok(result) : BadRequest(result);
    }
}
