using FluentValidation;
using LibraryManagement.Business.DTOs.Loan;

namespace LibraryManagement.Business.Validators;

public class LoanReturnDtoValidator : AbstractValidator<LoanReturnDto>
{
    public LoanReturnDtoValidator()
    {
        RuleFor(x => x.LoanId)
            .GreaterThan(0).WithMessage("Geçerli bir ödünç işlemi Id'si gereklidir.");
    }
}