using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int key);
        Task<Account> CreateAsync(Account Account);
        Task<Account?> UpdateAsync(Account Account);
        Task<bool> DeleteAsync(int key);
    }
}