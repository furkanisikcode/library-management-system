using FluentValidation;
using LibraryManagement.Business.DTOs.Member;

namespace LibraryManagement.Business.Validators;

public class MemberCreateDtoValidator : AbstractValidator<MemberCreateDto>
{
    public MemberCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Üye adı boş olamaz.")
            .MaximumLength(50).WithMessage("Üye adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Üye soyadı boş olamaz.")
            .MaximumLength(50).WithMessage("Üye soyadı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(150).WithMessage("E-posta en fazla 150 karakter olabilir.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Geçersiz rol seçimi.");
    }
}