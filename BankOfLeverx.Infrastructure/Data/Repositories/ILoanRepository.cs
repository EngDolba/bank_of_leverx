using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int key);
        Task<Loan> CreateAsync(LoanDTO Loan);
        Task<Loan?> UpdateAsync(int key, LoanDTO Loan);
        Task<Loan?> PatchAsync(int key, LoanPatchDTO LoanPatch);
        Task<bool> DeleteAsync(int key);
    }
}