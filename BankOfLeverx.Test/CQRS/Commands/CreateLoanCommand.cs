using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Commands
{
    public record
        CreateLoanCommand(LoanDTO Loan) : IRequest<Loan>;

}
