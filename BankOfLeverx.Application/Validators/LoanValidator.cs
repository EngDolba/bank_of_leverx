using BankOfLeverx.Core.DTO;
using FluentValidation;

namespace BankOfLeverx.Application.Validators;
public class LoanValidator : AbstractValidator<LoanDTO>
{
    public LoanValidator()
    {
        RuleFor(loan => loan.Rate)
            .GreaterThan(0).LessThan(100);
        RuleFor(loan => loan.Amount)
            .GreaterThan(0);
        RuleFor(loan => loan.StartDate)
              .LessThan(loan => loan.EndDate);
        RuleFor(loan => loan.StartDate).Must(date => date < DateTime.Now);
    }
}
