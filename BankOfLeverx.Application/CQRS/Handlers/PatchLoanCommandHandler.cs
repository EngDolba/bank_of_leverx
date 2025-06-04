using AutoMapper;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;
using BankOfLeverx.Application.CQRS.Commands;

namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class PatchLoanCommandHandler : IRequestHandler<PatchLoanCommand, Loan?>
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;

        public PatchLoanCommandHandler(ILoanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Loan?> Handle(PatchLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _repository.GetByIdAsync(request.Key);
            if (loan is null)
                throw new KeyNotFoundException($"Loan with key {request.Key} not found.");

            _mapper.Map(request.LoanPatch, loan);

            return await _repository.UpdateAsync(loan);
        }
    }
}
