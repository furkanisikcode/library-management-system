using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class PenaltyRepository : Repository<Penalty>, IPenaltyRepository
{
    public PenaltyRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Penalty?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l.Book)
            .Include(p => p.Loan)
                .ThenInclude(l => l.Member)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<List<Penalty>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l.Book)
            .Include(p => p.Loan)
                .ThenInclude(l => l.Member)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<Penalty>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .Include(p => p.Loan)
                .ThenInclude(l => l.Book)
            .Include(p => p.Loan)
                .ThenInclude(l => l.Member)
            .Where(p => p.Loan.MemberId == memberId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }
}