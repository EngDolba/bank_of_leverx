using MediatR;

namespace BankOfLeverx.Application.CQRS.Commands
{

    public record DeleteLoanCommand(int Key) : IRequest<bool>;

}
