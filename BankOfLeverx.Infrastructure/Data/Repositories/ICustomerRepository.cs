using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int key);
        Task<Customer> CreateAsync(CustomerDTO Customer);
        Task<Customer?> UpdateAsync(int key, CustomerDTO Customer);
        Task<Customer?> PatchAsync(int key, CustomerPatchDTO CustomerPatch);
        Task<bool> DeleteAsync(int key);
    }
}