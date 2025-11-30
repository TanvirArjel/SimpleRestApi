using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Models;

namespace Test.Infrastructure;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly TestDbContext _context;
    public EmployeeRepository(TestDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Email email)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email);
    }
}
