using FluentValidation;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Core.DTO;

namespace BankOfLeverx.Application.Validators;
public class LoanValidator : AbstractValidator<LoanDTO>
{
    public LoanValidator()
    {
      RuleFor(loan => loan.Rate)
          .GreaterThan(0).LessThan(1);
      RuleFor(loan => loan.Amount)
          .GreaterThan(0);
      RuleFor(loan => loan.StartDate)
            .LessThan(loan => loan.EndDate);
      RuleFor(loan => loan.StartDate).Must(date => date < DateTime.Now);
    }
}
