using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class PublisherRepository : Repository<Publisher>, IPublisherRepository
{
    public PublisherRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Publisher?> GetByIdWithBooksAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<List<Publisher>> GetAllWithBooksAsync()
    {
        return await _dbSet
            .Include(p => p.Books)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }
    public async Task<(List<Publisher> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize)
    {
    var query = _dbSet
        .Include(p => p.Books)
        .Where(p => !p.IsDeleted);

    var totalCount = await query.CountAsync();

    var items = await query
        .OrderByDescending(p => p.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, totalCount);
    }
}