using Application.Features.Employees.Commands;
using Application.Features.Employees.Validators;
using Application.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
	public static class Extensions
	{
		public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
		{
			return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>))
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
