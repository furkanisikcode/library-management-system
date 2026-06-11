using FluentValidation;
using LibraryManagement.Business.DTOs.Loan;

namespace LibraryManagement.Business.Validators;

public class LoanCreateDtoValidator : AbstractValidator<LoanCreateDto>
{
    public LoanCreateDtoValidator()
    {
        RuleFor(x => x.BookId)
            .GreaterThan(0).WithMessage("Geçerli bir kitap seçilmelidir.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Geçerli bir üye seçilmelidir.");
    }
}