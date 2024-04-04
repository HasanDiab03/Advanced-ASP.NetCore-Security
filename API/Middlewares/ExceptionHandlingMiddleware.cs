using Application.Exceptions;
using Azure;
using Common;
using Common.Responses.Wrappers;
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
				IResponseWrapper responseWrapper;
				switch (ex)
				{
					case CustomValidationException cvex:
						response.StatusCode = (int)HttpStatusCode.BadRequest;
						responseWrapper = ResponseWrapper<string>.Fail(cvex.ErrorMessages);
						break;
					default:
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						responseWrapper = ResponseWrapper<string>.Fail(ex.Message);
						break;
				}
				await response.WriteAsync(JsonSerializer.Serialize(responseWrapper));
			}
		}
	}

}
