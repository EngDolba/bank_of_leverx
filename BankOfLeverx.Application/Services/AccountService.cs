using BankOfLeverx.Application.Exceptions;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Account>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Account?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<Account> CreateAsync(AccountDTO dto)
        {
            var account = new Account
            {
                Number = dto.Number,
                PlanCode = dto.PlanCode,
                Balance = dto.Balance,
                CustomerKey = dto.CustomerKey
            };

            return await _repository.CreateAsync(account);
        }

        public async Task<Account?> UpdateAsync(int key, AccountDTO dto)
        {
            var account = new Account
            {
                Key = key,
                Number = dto.Number,
                PlanCode = dto.PlanCode,
                Balance = dto.Balance,
                CustomerKey = dto.CustomerKey
            };
            var acc = await _repository.UpdateAsync(account);
            if (acc is null)
            {
                throw new KeyNotFoundException();
            }

            return acc;
        }

        public async Task<Account?> PatchAsync(int key, AccountPatchDTO dto)
        {
            var account = await _repository.GetByIdAsync(key);
            if (account is null)
                throw new KeyNotFoundException($"account with Key {key} not found.");

            if (dto.Number is not null)
                account.Number = dto.Number;
            if (dto.PlanCode is not null)
                account.PlanCode = dto.PlanCode;
            if (dto.Balance.HasValue)
                account.Balance = dto.Balance.Value;
            if (dto.CustomerKey.HasValue)
                account.CustomerKey = dto.CustomerKey.Value;

            return await _repository.UpdateAsync(account);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
        public async Task<Account?> AmountChange(int key, double amount)
        {
            var account = await GetByIdAsync(key);
            if (account is null)
            {
                throw new KeyNotFoundException($"Account with key {key} not found.");
            }
            if (account.Balance < amount)
                throw new InsufficientFundsException("Not enough funds");
            account.Balance += amount;
            var updatedAccount = await _repository.UpdateAsync(account);
            return updatedAccount;



        }
    }
}
