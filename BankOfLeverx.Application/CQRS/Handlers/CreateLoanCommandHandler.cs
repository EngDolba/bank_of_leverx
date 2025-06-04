using AutoMapper;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;
using BankOfLeverx.Application.CQRS.Commands;



namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, Loan>
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;

        public CreateLoanCommandHandler(ILoanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Loan> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = _mapper.Map<Loan>(request.Loan);
            return await _repository.CreateAsync(loan);
        }
    }
}
