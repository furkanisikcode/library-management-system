using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByIdWithLoansAsync(int id);
    Task<List<Member>> GetAllWithLoansAsync();
    Task<Member?> GetByEmailAsync(string email);
    Task<(List<Member> Items, int TotalCount)> GetPagedWithLoansAsync(int pageNumber, int pageSize);
}