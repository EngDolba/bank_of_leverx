using BankOfLeverx.Domain.Models;
using MediatR;


namespace BankOfLeverx.Application.CQRS.Queries
{
    public record GetLoanByIdQuery(int Key) : IRequest<Loan?>;





}
