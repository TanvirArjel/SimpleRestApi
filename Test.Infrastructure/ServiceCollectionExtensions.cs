using System;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Domain.Models;

namespace Test.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        // Determine connection string based on the operating system
        string connectionStringKey = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "TestDbWindowsConnection"
            : "TestDbDockerConnection";

        string connectionString = configuration.GetConnectionString(connectionStringKey)
            ?? throw new InvalidOperationException($"Connection string '{connectionStringKey}' not found.");

        services.AddDbContext<TestDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}
