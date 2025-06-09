using AutoMapper;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Domain.Models;
using MediatR;



namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, Loan>
    {
        private readonly ILoanService _service;
        private readonly IMapper _mapper;

        public CreateLoanCommandHandler(ILoanService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Loan> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {

            return await _service.CreateAsync(request.Loan);
        }
    }
}
