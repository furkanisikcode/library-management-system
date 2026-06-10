using FluentValidation;
using LibraryManagement.Business.DTOs.Book;

namespace LibraryManagement.Business.Validators;

public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kitap başlığı boş olamaz.")
            .MaximumLength(200).WithMessage("Kitap başlığı en fazla 200 karakter olabilir.");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN boş olamaz.")
            .Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN 10 veya 13 haneli sayı olmalıdır.");

        RuleFor(x => x.PublicationYear)
            .GreaterThan(0).WithMessage("Yayın yılı geçerli olmalıdır.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Yayın yılı gelecekte olamaz.");

        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage("Sayfa sayısı 0'dan büyük olmalıdır.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.");

        RuleFor(x => x.PublisherId)
            .GreaterThan(0).WithMessage("Geçerli bir yayınevi seçilmelidir.");

        RuleFor(x => x.AuthorIds)
            .NotEmpty().WithMessage("En az bir yazar belirtilmelidir.");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("En az bir kategori belirtilmelidir.");
    }
}