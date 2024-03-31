using Common.Requests;
using Common.Responses.Wrappers;
using Domain;
using MediatR;

namespace Application.Services.Employees
{
	public interface IEmployeeRepository
	{
		Task<ResponseWrapper<List<Employee>>> GetEmployees();
		Task<ResponseWrapper<Employee>> GetEmployeeById(int id);
		Task<ResponseWrapper<Employee>> CreateEmployee(Employee employee);
		Task<ResponseWrapper<Employee>> UpdateEmployee(int id, Employee employee);
		Task<ResponseWrapper<Unit>> DeleteEmployee(int id);
	}
}
