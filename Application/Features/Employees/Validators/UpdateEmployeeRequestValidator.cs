using Common.Requests;
using FluentValidation;

namespace Application.Features.Employees.Validators
{
	public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
	{
        public UpdateEmployeeRequestValidator()
        {
			RuleFor(c => c.FirstName)
				.NotEmpty()
				.MaximumLength(60);
			RuleFor(c => c.LastName)
				.NotEmpty()
				.MaximumLength(60);
			RuleFor(c => c.Email)
				.NotEmpty()
				.MaximumLength(100);
			RuleFor(c => c.Salary)
				.NotEmpty();
		}
    }
}
