using Application.Features.Employees.Commands;
using FluentValidation;

namespace Application.Features.Employees.Validators
{
	public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
	{
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(c => c.Command.FirstName)
                .NotEmpty()
                .MaximumLength(60);
			RuleFor(c => c.Command.LastName)
				.NotEmpty()
				.MaximumLength(60);
			RuleFor(c => c.Command.Email)
				.NotEmpty()
				.MaximumLength(100);
			RuleFor(c => c.Command.Salary)
				.NotEmpty();
		}
    }
}
