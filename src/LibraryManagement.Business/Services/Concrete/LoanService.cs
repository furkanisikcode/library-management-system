using AutoMapper;
using LibraryManagement.Business.DTOs.Loan;
using LibraryManagement.Business.Exceptions;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Settings;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Entities.Enums;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Business.Services.Concrete;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IPenaltyRepository _penaltyRepository;
    private readonly IMapper _mapper;
    private readonly LoanSettings _loanSettings;

    public LoanService(
        ILoanRepository loanRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IPenaltyRepository penaltyRepository,
        IMapper mapper,
        IOptions<LoanSettings> loanSettings)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _penaltyRepository = penaltyRepository;
        _mapper = mapper;
        _loanSettings = loanSettings.Value;
    }

    public async Task<LoanDetailDto?> GetByIdAsync(int id)
    {
        var loan = await _loanRepository.GetByIdWithDetailsAsync(id);
        if (loan == null) return null;
        return _mapper.Map<LoanDetailDto>(loan);
    }

    public async Task<List<LoanListDto>> GetAllAsync()
    {
        var loans = await _loanRepository.GetAllWithDetailsAsync();
        return _mapper.Map<List<LoanListDto>>(loans);
    }

    public async Task<LoanDetailDto> BorrowBookAsync(LoanCreateDto createDto)
    {
        var member = await _memberRepository.GetByIdAsync(createDto.MemberId);
        if (member == null)
            throw new NotFoundException($"Id={createDto.MemberId} olan üye bulunamadı.");

        if (!member.IsActive)
            throw new BusinessRuleException(
                $"'{member.FirstName} {member.LastName}' adlı üye aktif değil. Ödünç verilemez.");

        var book = await _bookRepository.GetByIdAsync(createDto.BookId);
        if (book == null)
            throw new NotFoundException($"Id={createDto.BookId} olan kitap bulunamadı.");

        if (book.StockQuantity <= 0)
            throw new BusinessRuleException($"'{book.Title}' kitabı şu anda stokta yok.");

        var activeLoanCount = await _loanRepository.GetActiveLoanCountByMemberAsync(createDto.MemberId);
        if (activeLoanCount >= _loanSettings.MaxActiveLoanPerMember)
            throw new BusinessRuleException(
                $"Bu üye aynı anda en fazla {_loanSettings.MaxActiveLoanPerMember} kitap ödünç alabilir. " +
                $"Şu anda {activeLoanCount} aktif ödüncü var.");

        var unpaidPenaltyTotal = await _loanRepository.GetUnpaidPenaltyTotalByMemberAsync(createDto.MemberId);
        if (unpaidPenaltyTotal > 0)
            throw new BusinessRuleException(
                $"Bu üyenin ödenmemiş {unpaidPenaltyTotal:C} tutarında cezası var. " +
                "Önce ceza ödemesi yapılmalı.");

        var loan = new Loan
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(_loanSettings.LoanDurationInDays),
            Status = LoanStatus.Active
        };

        book.StockQuantity--;
        _bookRepository.Update(book);

        await _loanRepository.AddAsync(loan);
        await _loanRepository.SaveChangesAsync();

        var createdLoan = await _loanRepository.GetByIdWithDetailsAsync(loan.Id);
        return _mapper.Map<LoanDetailDto>(createdLoan);
    }

    public async Task<LoanDetailDto> ReturnBookAsync(LoanReturnDto returnDto)
    {
        var loan = await _loanRepository.GetByIdWithDetailsAsync(returnDto.LoanId);
        if (loan == null)
            throw new NotFoundException($"Id={returnDto.LoanId} olan ödünç işlemi bulunamadı.");

        if (loan.Status != LoanStatus.Active)
            throw new BusinessRuleException(
                $"Bu ödünç işlemi zaten '{loan.Status}' durumunda, tekrar iade edilemez.");

        var now = DateTime.UtcNow;

        if (now > loan.DueDate)
        {
            var daysOverdue = (int)Math.Ceiling((now - loan.DueDate).TotalDays);
            var penaltyAmount = daysOverdue * _loanSettings.DailyPenaltyAmount;

            var penalty = new Penalty
            {
                LoanId = loan.Id,
                Amount = penaltyAmount,
                Reason = $"{daysOverdue} gün gecikme cezası (günlük {_loanSettings.DailyPenaltyAmount:C})",
                IsPaid = false
            };

            await _penaltyRepository.AddAsync(penalty);
        }

        loan.Status = LoanStatus.Returned;
        loan.ReturnDate = now;
        _loanRepository.Update(loan);

        loan.Book.StockQuantity++;
        _bookRepository.Update(loan.Book);

        await _loanRepository.SaveChangesAsync();

        var updatedLoan = await _loanRepository.GetByIdWithDetailsAsync(loan.Id);
        return _mapper.Map<LoanDetailDto>(updatedLoan);
    }
}