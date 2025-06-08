using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;


namespace BankOfLeverx.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int key);
        Task<Customer> CreateAsync(CustomerDTO dto);
        Task<Customer?> UpdateAsync(int key, CustomerDTO dto);
        Task<Customer?> PatchAsync(int key, CustomerPatchDTO dto);
        Task<bool> DeleteAsync(int key);
    }
}
