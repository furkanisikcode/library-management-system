using LibraryManagement.Business.DTOs.Loan;

namespace LibraryManagement.Business.Services.Abstract;

public interface ILoanService
{
    Task<LoanDetailDto?> GetByIdAsync(int id);
    Task<List<LoanListDto>> GetAllAsync();
    Task<LoanDetailDto> BorrowBookAsync(LoanCreateDto createDto);
    Task<LoanDetailDto> ReturnBookAsync(LoanReturnDto returnDto);
}