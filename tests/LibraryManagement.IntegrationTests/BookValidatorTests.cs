using FluentValidation.TestHelper;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.Validators;

namespace LibraryManagement.IntegrationTests;

public class BookValidatorTests
{
    private readonly BookCreateDtoValidator _validator = new();

    // Geçerli bir DTO üreten yardımcı (testlerde tek alanı bozup deneyeceğiz)
    private static BookCreateDto ValidDto() => new()
    {
        Title = "Temiz Kod",
        ISBN = "9781234567897",
        PublicationYear = 2020,
        PageCount = 300,
        StockQuantity = 5,
        Description = "Yazılım kitabı",
        PublisherId = 1,
        AuthorIds = new List<int> { 1 },
        CategoryIds = new List<int> { 1 }
    };

    [Fact]
    public void ValidDto_PassesValidation()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmptyTitle_FailsValidation()
    {
        var dto = ValidDto();
        dto.Title = "";

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    [Fact]
    public void InvalidISBN_FailsValidation()
    {
        var dto = ValidDto();
        dto.ISBN = "123"; // 10 veya 13 hane değil

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ISBN);
    }

    [Fact]
    public void FuturePublicationYear_FailsValidation()
    {
        var dto = ValidDto();
        dto.PublicationYear = DateTime.UtcNow.Year + 5; // gelecekte

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.PublicationYear);
    }

    [Fact]
    public void ZeroPageCount_FailsValidation()
    {
        var dto = ValidDto();
        dto.PageCount = 0; // 0'dan büyük olmalı

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.PageCount);
    }

    [Fact]
    public void NegativeStock_FailsValidation()
    {
        var dto = ValidDto();
        dto.StockQuantity = -1; // negatif olamaz

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.StockQuantity);
    }

    [Fact]
    public void EmptyAuthorIds_FailsValidation()
    {
        var dto = ValidDto();
        dto.AuthorIds = new List<int>(); // en az bir yazar gerekli

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.AuthorIds);
    }
}