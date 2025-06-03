// TransactionService.cs
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int key);
        Task<Transaction> CreateAsync(TransactionDTO dto);
        Task<Transaction?> UpdateAsync(int key, TransactionDTO dto);
        Task<Transaction?> PatchAsync(int key, TransactionPatchDTO dto);
        Task<bool> DeleteAsync(int key);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Transaction?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<Transaction> CreateAsync(TransactionDTO dto)
        {
            var transaction = new Transaction
            {
                AccountKey = dto.AccountKey,
                IsDebit = dto.IsDebit,
                Category = dto.Category,
                Amount = dto.Amount,
                Date = dto.Date,
                Comment = dto.Comment
            };

            return await _repository.CreateAsync(transaction);
        }

        public async Task<Transaction?> UpdateAsync(int key, TransactionDTO dto)
        {
            var transaction = new Transaction
            {
                Key = key,
                AccountKey = dto.AccountKey,
                IsDebit = dto.IsDebit,
                Category = dto.Category,
                Amount = dto.Amount,
                Date = dto.Date,
                Comment = dto.Comment
            };

            return await _repository.UpdateAsync(transaction);
        }

        public async Task<Transaction?> PatchAsync(int key, TransactionPatchDTO dto)
        {
            var transaction = await _repository.GetByIdAsync(key);
            if (transaction is null)
                return null;

            if (dto.AccountKey is not null)
                transaction.AccountKey = dto.AccountKey.Value;
            if (dto.IsDebit is not null)
                transaction.IsDebit = dto.IsDebit.Value;
            if (dto.Category is not null)
                transaction.Category = dto.Category;
            if (dto.Amount is not null)
                transaction.Amount = dto.Amount.Value;
            if (dto.Date is not null)
                transaction.Date = dto.Date.Value;
            if (dto.Comment is not null)
                transaction.Comment = dto.Comment;

            return await _repository.UpdateAsync(transaction);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
    }
}