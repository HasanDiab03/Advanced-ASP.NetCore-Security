using Application.Services.Employees;
using AutoMapper;
using Common.Requests;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Features.Employees.Commands
{
	public record CreateEmployeeCommand(CreateEmployeeRequest Command) : IRequest<ResponseWrapper<Employee>>;
	public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, ResponseWrapper<Employee>>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;

		public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
			_employeeRepository = employeeRepository;
			_mapper = mapper;
		}
        public async Task<ResponseWrapper<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
		{
			var employee = _mapper.Map<Employee>(request.Command);
			return await _employeeRepository.CreateEmployee(employee);
		}
	}
}
