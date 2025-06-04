using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Commands
{
    public record PatchLoanCommand(int Key, LoanPatchDTO LoanPatch) : IRequest<Loan?>;

}
