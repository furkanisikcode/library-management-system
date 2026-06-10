using FluentValidation;
using LibraryManagement.Business.DTOs.Publisher;

namespace LibraryManagement.Business.Validators;

public class PublisherUpdateDtoValidator : AbstractValidator<PublisherUpdateDto>
{
    public PublisherUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir yayınevi Id'si gereklidir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Yayınevi adı boş olamaz.")
            .MaximumLength(200).WithMessage("Yayınevi adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

        RuleFor(x => x.Website)
            .MaximumLength(200).WithMessage("Website adresi en fazla 200 karakter olabilir.");
    }
}