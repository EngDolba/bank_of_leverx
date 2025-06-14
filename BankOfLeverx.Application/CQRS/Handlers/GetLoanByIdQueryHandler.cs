﻿using BankOfLeverx.Application.CQRS.Queries;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using MediatR;

namespace BankOfLeverx.Application.CQRS.Handlers
{
    public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, Loan?>
    {
        private readonly ILoanRepository _repository;

        public GetLoanByIdQueryHandler(ILoanRepository repository)
        {
            _repository = repository;
        }

        public Task<Loan?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(request.Key);
        }
    }
}
