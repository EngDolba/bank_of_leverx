using AutoMapper;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class PatchLoanCommandHandler : IRequestHandler<PatchLoanCommand, Loan?>
    {
        private readonly ILoanService _service;
        private readonly IMapper _mapper;

        public PatchLoanCommandHandler(ILoanService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Loan?> Handle(PatchLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _service.GetByIdAsync(request.Key);
            if (loan is null)
                throw new KeyNotFoundException($"Loan with key {request.Key} not found.");
            var ln=  _mapper.Map<LoanDTO>(loan);
            ln = _mapper.Map(request.LoanPatch, ln);
            return await _service.UpdateAsync(request.Key, ln);
        }
    }
}
