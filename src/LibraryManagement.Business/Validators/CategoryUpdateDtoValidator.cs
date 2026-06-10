using FluentValidation;
using LibraryManagement.Business.DTOs.Category;

namespace LibraryManagement.Business.Validators;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir kategori Id'si gereklidir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş olamaz.")
            .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
    }
}