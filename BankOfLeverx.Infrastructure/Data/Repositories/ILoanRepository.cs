using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Infrastructure.Data.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int key);
        Task<Loan> CreateAsync(Loan Loan);
        Task<Loan?> UpdateAsync(Loan Loan);
        Task<bool> DeleteAsync(int key);
    }
}