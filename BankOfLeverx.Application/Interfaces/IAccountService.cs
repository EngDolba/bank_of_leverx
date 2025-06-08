using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int key);
        Task<Account> CreateAsync(AccountDTO dto);
        Task<Account?> UpdateAsync(int key, AccountDTO dto);
        Task<Account?> PatchAsync(int key, AccountPatchDTO dto);
        Task<bool> DeleteAsync(int key);
        Task<Account?> AmountChange(int key, double amount);

    }
}
