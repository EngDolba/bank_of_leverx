// Infrastructure/Data/Repositories/IEmployeeRepository.cs
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int key);
        Task<Employee> CreateAsync(EmployeeDTO employee);
        Task<Employee?> UpdateAsync(int key, EmployeeDTO employee);
        Task<Employee?> PatchAsync(int key, EmployeePatchDTO employeePatch);
        Task<bool> DeleteAsync(int key);
    }
}