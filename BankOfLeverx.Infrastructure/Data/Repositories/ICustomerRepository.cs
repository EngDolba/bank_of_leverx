using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int key);
        Task<Customer> CreateAsync(Customer Customer);
        Task<Customer?> UpdateAsync(Customer Customer);
        Task<bool> DeleteAsync(int key);
    }
}