using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int key);
        Task<Loan> CreateAsync(LoanDTO dto);

        Task<Loan?> SubtractInterestAsync(int key);

        Task<Loan?> UpdateAsync(int key, LoanDTO dto);
        Task<Loan?> PatchAsync(int key, LoanPatchDTO dto);
        Task<bool> DeleteAsync(int key);
        double calculateInterest(Loan loan);

    }
}
