// ITransactionService.cs
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int key);
        Task<Transaction> CreateAsync(TransactionDTO dto);
        Task<Transaction?> UpdateAsync(int key, TransactionDTO dto);
        Task<Transaction?> PatchAsync(int key, TransactionPatchDTO dto);
        Task<bool> DeleteAsync(int key);
        Task<Transaction?> processTransaction(int accountKey, double amount);

    }
}