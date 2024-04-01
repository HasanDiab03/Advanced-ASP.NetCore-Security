﻿namespace Common.Responses.Identity
{
	public class UserResponse
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public bool IsActive { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PhoneNumber { get; set; }
	}
}
