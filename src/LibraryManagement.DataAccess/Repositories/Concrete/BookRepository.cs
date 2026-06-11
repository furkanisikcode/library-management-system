using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Book?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<List<Book>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Where(b => !b.IsDeleted)
            .ToListAsync();
    }
    public async Task<(List<Book> Items, int TotalCount)> GetPagedWithDetailsAsync(int pageNumber, int pageSize)
    {
    var query = _dbSet
        .Include(b => b.Publisher)
        .Include(b => b.Authors)
        .Include(b => b.Categories)
        .Where(b => !b.IsDeleted);

    var totalCount = await query.CountAsync();

    var items = await query
        .OrderByDescending(b => b.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, totalCount);
    }
}