using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetByIdWithDetailsAsync(int id);
    Task<List<Loan>> GetAllWithDetailsAsync();
    Task<int> GetActiveLoanCountByMemberAsync(int memberId);
    Task<decimal> GetUnpaidPenaltyTotalByMemberAsync(int memberId);
}