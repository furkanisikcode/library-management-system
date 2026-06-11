using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Include(l => l.Penalties.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
    }

    public async Task<List<Loan>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Include(l => l.Penalties.Where(p => !p.IsDeleted))
            .Where(l => !l.IsDeleted)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    public async Task<int> GetActiveLoanCountByMemberAsync(int memberId)
    {
        return await _dbSet
            .CountAsync(l => l.MemberId == memberId
                          && l.Status == LoanStatus.Active
                          && !l.IsDeleted);
    }

    public async Task<decimal> GetUnpaidPenaltyTotalByMemberAsync(int memberId)
    {
        return await _context.Set<Penalty>()
            .Where(p => p.Loan.MemberId == memberId
                     && !p.IsPaid
                     && !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }
}