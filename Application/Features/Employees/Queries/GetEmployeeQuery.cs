using Application.Services.Employees;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Features.Employees.Queries
{
	public record GetEmployeeQuery(int Id) : IRequest<ResponseWrapper<Employee>>;
	public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, ResponseWrapper<Employee>>
	{
		private readonly IEmployeeRepository _employeeRepository;

		public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
        {
			_employeeRepository = employeeRepository;
		}
		public async Task<ResponseWrapper<Employee>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
			=> await _employeeRepository.GetEmployeeById(request.Id);
	}
}
