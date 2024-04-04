using Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Pipelines
{
	public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TRequest>, IValidateMe
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);
				List<string> errors = new();
				var validationResults = await Task.WhenAll(_validators
					.Select(vr => vr.ValidateAsync(context, cancellationToken)));
				var failures = validationResults.SelectMany(vr => vr.Errors)
					.Where(f => f is not null).ToList();
				if(failures.Any())
				{
					errors.AddRange(failures.Select(x => x.ErrorMessage));
					throw new CustomValidationException(errors, "One or more validation errors occured");
				}
			}
			return await next();
		}
	}
}
