using Application.Pipelines;
using Application.Services.Employees;
using AutoMapper;
using Common.Requests;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Features.Employees.Commands
{
	public record UpdateEmployeeCommand(int Id, UpdateEmployeeRequest Command) : IRequest<ResponseWrapper<Employee>>, IValidateMe;
	public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ResponseWrapper<Employee>>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;

		public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
			_employeeRepository = employeeRepository;
			_mapper = mapper;
		}
        public async Task<ResponseWrapper<Employee>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
		{
			var mappedEmployee = _mapper.Map<Employee>(request.Command);
			return await _employeeRepository.UpdateEmployee(request.Id, mappedEmployee);
		}
	}
}
