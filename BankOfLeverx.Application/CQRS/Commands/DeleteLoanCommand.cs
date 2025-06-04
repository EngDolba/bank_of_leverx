using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOfLeverx.Application.CQRS.Commands
{

    public record DeleteLoanCommand(int Key) : IRequest<bool>;

}
