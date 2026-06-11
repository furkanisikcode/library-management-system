using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Author?> GetByIdWithBooksAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<List<Author>> GetAllWithBooksAsync()
    {
        return await _dbSet
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted)
            .ToListAsync();
    }
    public async Task<(List<Author> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize)
    {
    var query = _dbSet
        .Include(a => a.Books)
        .Where(a => !a.IsDeleted);

    var totalCount = await query.CountAsync();

    var items = await query
        .OrderByDescending(a => a.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, totalCount);
    }
}