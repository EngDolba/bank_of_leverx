using AutoMapper;
using BankOfLeverx.Application.Exceptions;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Services
{


    public class LoanPaymentService : ILoanPaymentService
    {
        private readonly ILoanService _loanService;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public LoanPaymentService(ILoanService loanService, IMapper mapper, ITransactionService transactionService)
        {
            _loanService = loanService;
            _mapper = mapper;
            _transactionService = transactionService;
        }

      
        public async Task<Loan?> SubtractInterestAsync(int key)
        {
            var loan = await _loanService.GetByIdAsync(key);
            if (loan is null)
                throw new KeyNotFoundException($"Loan with key {key} not found.");
            double interest = _loanService.calculateInterest(loan);
            var transaction = await _transactionService.processTransaction(loan.AccountKey, -interest);
            var loanDTO = _mapper.Map<LoanDTO>(loan);
            var updatedLoan = await _loanService.UpdateAsync(key, loanDTO);
            return updatedLoan;
        }
    }
}
