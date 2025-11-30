using System;
using System.Threading.Tasks;

namespace Test.Domain.Models;

public interface IEmployeeRepository
{
    Task<bool> ExistsAsync(Email email);
}
