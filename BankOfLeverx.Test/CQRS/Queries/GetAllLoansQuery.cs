using BankOfLeverx.Domain.Models;
using MediatR;


namespace BankOfLeverx.Application.CQRS.Queries
{
    public record GetAllLoansQuery() : IRequest<IEnumerable<Loan>>;

}
