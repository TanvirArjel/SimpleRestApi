using Microsoft.EntityFrameworkCore;
using Test.Domain.Models;

namespace Test.Infrastructure;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
