using Application.Services.Employees;
using Common.Responses.Wrappers;
using MediatR;

namespace Application.Features.Employees.Commands
{
	public record DeleteEmployeeCommand(int Id) : IRequest<ResponseWrapper<Unit>>;
	public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ResponseWrapper<Unit>>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
			_employeeRepository = employeeRepository;
		}
		public async Task<ResponseWrapper<Unit>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
			=> await _employeeRepository.DeleteEmployee(request.Id);
	}
}
