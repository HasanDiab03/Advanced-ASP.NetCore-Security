namespace Domain
{
	public class Employee
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public void Update(Employee updatedEmployee)
        {
            FirstName = updatedEmployee.FirstName;
            LastName = updatedEmployee.LastName;
            Email = updatedEmployee.Email;
            Salary = updatedEmployee.Salary;
        }
    }
}