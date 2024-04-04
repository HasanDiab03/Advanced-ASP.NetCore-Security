using Application.Exceptions;
using Common;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try 
			{
				await _next(context);
			}
			catch(Exception ex)
			{
				var response = context.Response;
				response.ContentType = "application/json";
				Error err = new Error();
				switch (ex)
				{
					case CustomValidationException cvex:
						response.StatusCode = (int)HttpStatusCode.BadRequest;
						err.ErrorMessages = cvex.ErrorMessages;
						err.FriendlyMessage = cvex.FriendlyErrorMessage;
						break;
					default:
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						err.FriendlyMessage = ex.Message;
						break;
				}
				await response.WriteAsync(JsonSerializer.Serialize(err));
			}
		}
	}

}
