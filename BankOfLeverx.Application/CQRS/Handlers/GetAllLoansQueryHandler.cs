using BankOfLeverx.Application.CQRS.Queries;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, IEnumerable<Loan>>
    {
        private readonly ILoanService _service;

        public GetAllLoansQueryHandler(ILoanService service)
        {
            _service = service;
        }

        public Task<IEnumerable<Loan>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
        {
            return _service.GetAllAsync();
        }
    }
}
