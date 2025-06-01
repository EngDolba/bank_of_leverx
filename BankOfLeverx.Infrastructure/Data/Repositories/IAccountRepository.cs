using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int key);
        Task<Account> CreateAsync(AccountDTO Account);
        Task<Account?> UpdateAsync(int key, AccountDTO Account);
        Task<Account?> PatchAsync(int key, AccountPatchDTO AccountPatch);
        Task<bool> DeleteAsync(int key);
    }
}