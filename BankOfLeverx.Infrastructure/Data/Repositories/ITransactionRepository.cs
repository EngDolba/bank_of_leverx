using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int key);
        Task<Transaction> CreateAsync(Transaction Transaction);
        Task<Transaction?> UpdateAsync(Transaction Transaction);
        Task<bool> DeleteAsync(int key);
    }
}