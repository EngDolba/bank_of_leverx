using AutoMapper;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;


namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class SubtractInterestCommandHandler : IRequestHandler<SubtractInterestCommand, Loan?>
    {
        private readonly ILoanService _service;
        private readonly IMapper _mapper;

        public SubtractInterestCommandHandler(ILoanService service, IMapper mapper)
        {
          _service = service;
          _mapper = mapper;
        }

        public async Task<Loan?> Handle(SubtractInterestCommand request, CancellationToken cancellationToken)
        {
                return await _service.SubtractInterestAsync(request.LoanKey);
        }

       
    }
}
