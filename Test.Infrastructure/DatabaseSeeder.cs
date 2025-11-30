using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test.Domain.Models;

namespace Test.Infrastructure;

internal sealed class DatabaseSeeder(
    TestDbContext dbContext,
    IEmployeeRepository employeeRepository,
    ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync()
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            logger.LogInformation("Starting database seeding...");

            // Check if data already exists
            bool hasEmployees = await dbContext.Set<Employee>().AnyAsync();

            if (hasEmployees)
            {
                logger.LogInformation("Database already contains employees. Skipping employee seeding.");
            }
            else
            {
                logger.LogInformation("Seeding employees...");
                await SeedEmployeesAsync();
            }

            await transaction.CommitAsync();
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "An error occurred while seeding the database. Transaction rolled back.");
            throw;
        }
    }

    private async Task SeedEmployeesAsync()
    {
        // Get departments
        List<Employee> employees = new();

        var employeeData = new[]
        {
            ("john.doe@cleanhr.com"),
            ("jane.smith@cleanhr.com"),
            ("michael.johnson@cleanhr.com"),
            ("emily.davis@cleanhr.com"),
            ("robert.brown@cleanhr.com")
        };

        foreach (var email in employeeData)
        {
            var emailValueObject = new Email(email);
            var employee = await Employee.CreateAsync(
                employeeRepository,
                emailValueObject);

            if (employee != null)
            {
                employees.Add(employee);
            }
            else if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Failed to create employee '{Email}'", email);
            }
        }

        if (employees.Count > 0)
        {
            await dbContext.Set<Employee>().AddRangeAsync(employees);
            await dbContext.SaveChangesAsync();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Seeded {Count} employees", employees.Count);
            }
        }
        else
        {
            logger.LogWarning("No employees were seeded due to validation errors");
        }
    }
}
