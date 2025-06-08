using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Commands
{
    public record
        SubtractInterestCommand(int LoanKey) : IRequest<Loan?>;

}
