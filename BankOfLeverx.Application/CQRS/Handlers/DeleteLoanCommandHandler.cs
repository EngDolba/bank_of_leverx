using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;
using BankOfLeverx.Application.CQRS.Commands;


namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, bool>
    {
        private readonly ILoanRepository _repository;

        public DeleteLoanCommandHandler(ILoanRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(request.Key);
        }
    }
}
