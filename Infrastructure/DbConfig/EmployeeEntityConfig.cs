﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig
{
	public class EmployeeEntityConfig : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.ToTable("Employees", SchemaNames.HR)
				.HasIndex(e => e.FirstName)
				.HasDatabaseName("IX_Employees_FirstName");
			builder.HasIndex(e => e.LastName)
				.HasDatabaseName("IX_Employees_LastName");
		}
	}
}
