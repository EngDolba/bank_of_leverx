using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;
using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.Interfaces;


namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, bool>
    {
        private readonly ILoanService _service;

        public DeleteLoanCommandHandler(ILoanService service)
        {
            _service = service;
        }

        public Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
        {
            return _service.DeleteAsync(request.Key);
        }
    }
}
