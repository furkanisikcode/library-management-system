using FluentValidation;
using LibraryManagement.Business.DTOs.Author;

namespace LibraryManagement.Business.Validators;

public class AuthorUpdateDtoValidator : AbstractValidator<AuthorUpdateDto>
{
    public AuthorUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir yazar Id'si gereklidir.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Yazar adı boş olamaz.")
            .MaximumLength(50).WithMessage("Yazar adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Yazar soyadı boş olamaz.")
            .MaximumLength(50).WithMessage("Yazar soyadı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Biography)
            .MaximumLength(2000).WithMessage("Biyografi en fazla 2000 karakter olabilir.");

        RuleFor(x => x.Nationality)
            .MaximumLength(50).WithMessage("Milliyet en fazla 50 karakter olabilir.");

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Doğum tarihi gelecekte olamaz.")
            .When(x => x.BirthDate.HasValue);
    }
}