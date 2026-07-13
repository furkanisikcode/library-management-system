using LibraryManagement.Business.DTOs.Loan;
using LibraryManagement.Business.Exceptions;
using LibraryManagement.Business.Services.Concrete;
using LibraryManagement.Business.Settings;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;
using LibraryManagement.Business.Mappings;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryManagement.IntegrationTests;

public class LoanServiceTests
{
    private const string TestConnectionString =
        "Host=localhost;Port=5432;Database=LibraryDB_Test;Username=postgres;Password=postgres";

    // Her test için taze bir LoanService + context kur
    private static LoanService CreateService(out LibraryDbContext context)
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseNpgsql(TestConnectionString)
            .Options;

        context = new LibraryDbContext(options);
        context.Database.EnsureCreated();

        var loanRepo = new LoanRepository(context);
        var bookRepo = new BookRepository(context);
        var memberRepo = new MemberRepository(context);
        var penaltyRepo = new PenaltyRepository(context);

        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddMaps(typeof(BookProfile).Assembly), NullLoggerFactory.Instance);
        var mapper = mapperConfig.CreateMapper();

        var loanSettings = Options.Create(new LoanSettings
        {
            MaxActiveLoanPerMember = 5,
            LoanDurationInDays = 14,
            DailyPenaltyAmount = 5.00m
        });

        return new LoanService(loanRepo, bookRepo, memberRepo, penaltyRepo, mapper, loanSettings);
    }

    [Fact]
    public async Task BorrowBook_WithNonExistentMember_ThrowsNotFoundException()
    {
        var service = CreateService(out var context);

        var dto = new LoanCreateDto
        {
            MemberId = 999999, // olmayan üye
            BookId = 1
        };

        // Olmayan üye ile ödünç almak NotFoundException fırlatmalı
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.BorrowBookAsync(dto));

        context.Dispose();
    }
    [Fact]
    public async Task BorrowBook_WithValidData_DecreasesStock()
    {
        var service = CreateService(out var context);

        // Seed: yayıncı + stoklu kitap + aktif üye
        var publisher = new LibraryManagement.Entities.Concrete.Publisher { Name = "Test Yayınevi" };
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();

        var book = new LibraryManagement.Entities.Concrete.Book
        {
            Title = "Test Kitap",
            ISBN = "978-000-000",
            PublicationYear = 2020,
            StockQuantity = 3,
            PublisherId = publisher.Id
        };
        context.Books.Add(book);

        var member = new LibraryManagement.Entities.Concrete.Member
        {
            FirstName = "Aktif",
            LastName = "Uye",
            Email = $"aktif_{System.Guid.NewGuid():N}@test.com",
            PasswordHash = "x",
            IsActive = true
        };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var dto = new LoanCreateDto { MemberId = member.Id, BookId = book.Id };

        // Ödünç al
        await service.BorrowBookAsync(dto);

        // Stok 3'ten 2'ye düşmeli
        var updatedBook = await context.Books.FindAsync(book.Id);
        Assert.Equal(2, updatedBook!.StockQuantity);

        context.Dispose();
    }

    [Fact]
    public async Task BorrowBook_WithZeroStock_ThrowsBusinessRuleException()
    {
        var service = CreateService(out var context);

        var publisher = new LibraryManagement.Entities.Concrete.Publisher { Name = "Test Yayınevi 2" };
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();

        var book = new LibraryManagement.Entities.Concrete.Book
        {
            Title = "Stoksuz Kitap",
            ISBN = "978-111-111",
            PublicationYear = 2021,
            StockQuantity = 0, // stok yok
            PublisherId = publisher.Id
        };
        context.Books.Add(book);

        var member = new LibraryManagement.Entities.Concrete.Member
        {
            FirstName = "Aktif",
            LastName = "Uye",
            Email = $"aktif_{System.Guid.NewGuid():N}@test.com",
            PasswordHash = "x",
            IsActive = true
        };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var dto = new LoanCreateDto { MemberId = member.Id, BookId = book.Id };

        // Stok 0 → BusinessRuleException beklenir
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.BorrowBookAsync(dto));

        context.Dispose();
    }
}