using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IPenaltyRepository : IRepository<Penalty>
{
    Task<Penalty?> GetByIdWithDetailsAsync(int id);
    Task<List<Penalty>> GetAllWithDetailsAsync();
    Task<List<Penalty>> GetByMemberIdAsync(int memberId);
}