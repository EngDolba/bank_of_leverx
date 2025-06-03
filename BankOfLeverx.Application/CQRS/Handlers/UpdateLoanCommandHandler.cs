using AutoMapper;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;


namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class UpdateLoanCommandHandler : IRequestHandler<UpdateLoanCommand, Loan?>
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLoanCommandHandler(ILoanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Loan?> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = _mapper.Map<Loan>(request.Loan);
            loan.Key = request.Key;
            var updatedLoan = await _repository.UpdateAsync(loan);

            if (updatedLoan is null)
                throw new KeyNotFoundException($"Loan with key {request.Key} not found.");

            return updatedLoan;
        }
    }
}
