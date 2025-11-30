using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test.Domain.Models;

namespace Test.Infrastructure;


public static class ApplicationBuilderExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using IServiceScope serviceScope = app.Services.CreateScope();
        TestDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TestDbContext>();

        // TODO: Comment out this if you have SQL server installed on your machine.
        dbContext.Database.Migrate();
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using IServiceScope serviceScope = app.Services.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;

        var dbContext = serviceProvider.GetRequiredService<TestDbContext>();
        var employeeRepository = serviceProvider.GetRequiredService<IEmployeeRepository>();

        var logger = serviceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();

        var seeder = new DatabaseSeeder(
            dbContext,
            employeeRepository,
            logger);

        await seeder.SeedAsync();
    }
}
