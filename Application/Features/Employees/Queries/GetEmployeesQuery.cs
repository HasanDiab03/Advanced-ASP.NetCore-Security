using Application.Services.Employees;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Features.Employees.Queries
{
	public record GetEmployeesQuery: IRequest<ResponseWrapper<List<Employee>>>;
	public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, ResponseWrapper<List<Employee>>>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        {
			_employeeRepository = employeeRepository;
		}
		public async Task<ResponseWrapper<List<Employee>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
			=> await _employeeRepository.GetEmployees();
	}
}
