using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Commands
{
    public record UpdateLoanCommand(int Key, LoanDTO Loan) : IRequest<Loan?>;

}
