using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int key);
        Task<Transaction> CreateAsync(TransactionDTO Transaction);
        Task<Transaction?> UpdateAsync(int key, TransactionDTO Transaction);
        Task<Transaction?> PatchAsync(int key, TransactionPatchDTO TransactionPatch);
        Task<bool> DeleteAsync(int key);
    }
}