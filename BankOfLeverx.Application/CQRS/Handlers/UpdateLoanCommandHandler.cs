using AutoMapper;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Domain.Models;
using MediatR;


namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class UpdateLoanCommandHandler : IRequestHandler<UpdateLoanCommand, Loan?>
    {
        private readonly ILoanService _service;
        private readonly IMapper _mapper;

        public UpdateLoanCommandHandler(ILoanService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Loan?> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = _mapper.Map<Loan>(request.Loan);
            loan.Key = request.Key;
            var updatedLoan = await _service.UpdateAsync(request.Key, request.Loan);

            if (updatedLoan is null)
                throw new KeyNotFoundException($"Loan with key {request.Key} not found.");

            return updatedLoan;
        }
    }
}
