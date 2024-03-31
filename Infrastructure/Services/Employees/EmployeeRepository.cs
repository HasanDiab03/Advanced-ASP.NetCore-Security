using Application.Services.Employees;
using AutoMapper;
using Common.Requests;
using Common.Responses.Wrappers;
using Domain;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Employees
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public EmployeeRepository(ApplicationDbContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public async Task<ResponseWrapper<Employee>> CreateEmployee(Employee employee)
		{
			await _context.Employees.AddAsync(employee);
			var success = await _context.SaveChangesAsync() > 0;
			if(!success)
			{
				return ResponseWrapper<Employee>.Fail("Failed To Create Employee");
			}
			return ResponseWrapper<Employee>.Success(employee);
		}

		public async Task<ResponseWrapper<Unit>> DeleteEmployee(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if(employee is null)
			{
				return ResponseWrapper<Unit>.Fail($"No Employee with id:{id} exists");
			}
			_context.Employees.Remove(employee);
			var success = await _context.SaveChangesAsync() > 0;
			if(!success)
			{
				return ResponseWrapper<Unit>.Fail("Failed To Delete Employee");
			}
			return ResponseWrapper<Unit>.Success(Unit.Value);
		}

		public async Task<ResponseWrapper<Employee>> GetEmployeeById(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if(employee is null)
			{
				return ResponseWrapper<Employee>.Fail($"Employee with id:{id} does not exist");
			}
			return ResponseWrapper<Employee>.Success(employee);
		}

		public async Task<ResponseWrapper<List<Employee>>> GetEmployees()
		{
			var employees = await _context.Employees.ToListAsync();
			if(employees is null)
			{
				return ResponseWrapper<List<Employee>>.Fail("Failed To Fetch Employees");
			}
			return ResponseWrapper<List<Employee>>.Success(employees);
		}

		public async Task<ResponseWrapper<Employee>> UpdateEmployee(int id, Employee employee)
		{
			var employeeExists = await _context.Employees.FindAsync(id);
			if(employeeExists is null)
			{
				return ResponseWrapper<Employee>.Fail($"Employee with id:{id} does not exist");
			}
			employeeExists.Update(employee);
			_context.Employees.Update(employeeExists);
			var success = await _context.SaveChangesAsync() > 0;
			if(!success)
			{
				return ResponseWrapper<Employee>.Fail("Failed To Update Employee");
			}
			return ResponseWrapper<Employee>.Success(employeeExists);
		}
	}
}
