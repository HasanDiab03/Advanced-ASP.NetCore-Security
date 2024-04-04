using Application.Features.Employees.Commands;
using Application.Services.Employees;
using Domain;
using FluentValidation;

namespace Application.Features.Employees.Validators
{
	public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
	{
        public UpdateEmployeeCommandValidator(IEmployeeRepository employeeRepository)
        {
            RuleFor(c => c.Id)
                .MustAsync(async (id, ct) => (await employeeRepository.GetEmployeeById(id)).ResponseData 
                is Employee employeeExists 
                && employeeExists.Id == id)
                .WithMessage("No Emp Exists");
            RuleFor(c => c.Command)
                .SetValidator(new UpdateEmployeeRequestValidator());
		}
    }
}
