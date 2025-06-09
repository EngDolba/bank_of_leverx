using AutoMapper;
using Azure.Core;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{


    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            var loan = _mapper.Map<Loan>(dto);
            return await _repository.CreateAsync(loan);
        }

        public async Task<Loan?> UpdateAsync(int key, LoanDTO dto)
        {
            var loan = _mapper.Map<Loan>(dto);
            loan.Key = key;
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
            if (dto is null)
                Console.WriteLine("aaaaa");
            if (_mapper is null)
                Console.WriteLine("nini");
            loan = _mapper.Map(dto, loan);

            return await _repository.UpdateAsync(loan);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
        public async Task<Loan?> SubtractInterestAsync(int key)
        {
            var loan = await GetByIdAsync(key);
            if (loan is null)
                throw new KeyNotFoundException($"Loan with key {key} not found.");
            double interest = loan.Amount * (loan.Rate/1200);
            double amt = Math.Max(loan.Amount - interest, 0);
            Console.WriteLine(interest+"int");
            Console.WriteLine(amt + "amt");
            loan.Amount = amt;
            var updatedLoan = await UpdateAsync(key, _mapper.Map<LoanDTO>(loan));
            return updatedLoan;
        }
    }
}
