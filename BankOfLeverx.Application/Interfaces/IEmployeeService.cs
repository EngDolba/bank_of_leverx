using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int key);
        Task<Employee> CreateAsync(EmployeeDTO dto);
        Task<Employee?> UpdateAsync(int key, EmployeeDTO dto);
        Task<Employee?> PatchAsync(int key, EmployeePatchDTO dto);
        Task<bool> DeleteAsync(int key);
    }
}
