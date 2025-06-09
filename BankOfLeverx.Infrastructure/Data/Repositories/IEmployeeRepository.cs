// Infrastructure/Data/Repositories/IEmployeeRepository.cs
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int key);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<Employee?> PatchAsync(Employee employeePatch);
        Task<bool> DeleteAsync(int key);
    }
}