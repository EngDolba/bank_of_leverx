using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;

namespace BankOfLeverx.Application.Interfaces
{
    public interface ILoanPaymentService
    {
      Task<Loan?> SubtractInterestAsync(int key);
    }
}
