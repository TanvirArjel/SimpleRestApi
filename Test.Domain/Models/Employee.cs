using System;
using System.Threading.Tasks;

namespace Test.Domain.Models;

public class Employee
{
    public Guid Id { get; private set; }

    public Email Email { get; private set; }

    public static async Task<Employee> CreateAsync(IEmployeeRepository employeeRepository, Email email)
    {
        var exists = await employeeRepository.ExistsAsync(email);

        if (exists)
        {
            throw new InvalidOperationException("An employee with the given email already exists.");
        }

        return new Employee
        {
            Id = Guid.NewGuid(),
            Email = email
        };
    }
}
