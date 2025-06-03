using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repository;

        public LoanService(ILoanRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Loan>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Loan?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<Loan> CreateAsync(LoanDTO dto)
        {
            var loan = new Loan
            {
                Amount = dto.Amount,
                InitialAmount = dto.InitialAmount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Rate = dto.Rate,
                Type = dto.Type,
                BankerKey = dto.BankerKey,
                AccountKey = dto.AccountKey
            };

            return await _repository.CreateAsync(loan);
        }

        public async Task<Loan?> UpdateAsync(int key, LoanDTO dto)
        {
            var loan = new Loan
            {
                Key = key,
                Amount = dto.Amount,
                InitialAmount = dto.InitialAmount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Rate = dto.Rate,
                Type = dto.Type,
                BankerKey = dto.BankerKey,
                AccountKey = dto.AccountKey
            };
            var ln = await _repository.UpdateAsync(loan);
            if (ln is null)
            {
                throw new KeyNotFoundException();
            }

            return ln;
        }

        public async Task<Loan?> PatchAsync(int key, LoanPatchDTO dto)
        {
            var loan = await _repository.GetByIdAsync(key);
            if (loan is null)
                throw new KeyNotFoundException();

            if (dto.Amount is not null)
                loan.Amount = dto.Amount.Value;
            if (dto.InitialAmount is not null)
                loan.InitialAmount = dto.InitialAmount.Value;
            if (dto.StartDate is not null)
                loan.StartDate = dto.StartDate.Value;
            if (dto.EndDate is not null)
                loan.EndDate = dto.EndDate.Value;
            if (dto.Rate is not null)
                loan.Rate = dto.Rate.Value;
            if (dto.Type is not null)
                loan.Type = dto.Type;
            if (dto.BankerKey is not null)
                loan.BankerKey = dto.BankerKey.Value;
            if (dto.AccountKey is not null)
                loan.AccountKey = dto.AccountKey.Value;

            return await _repository.UpdateAsync(loan);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
    }
}
