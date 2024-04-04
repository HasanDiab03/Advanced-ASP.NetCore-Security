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
			return services
				.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
		}
	}
}
