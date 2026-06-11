using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByIdWithLoansAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Loans)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }

    public async Task<List<Member>> GetAllWithLoansAsync()
    {
        return await _dbSet
            .Include(m => m.Loans)
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Email == email && !m.IsDeleted);
    }
    public async Task<(List<Member> Items, int TotalCount)> GetPagedWithLoansAsync(int pageNumber, int pageSize)
    {
    var query = _dbSet
        .Include(m => m.Loans)
        .Where(m => !m.IsDeleted);

    var totalCount = await query.CountAsync();

    var items = await query
        .OrderByDescending(m => m.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, totalCount);
    }
}