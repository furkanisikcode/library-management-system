using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetByIdWithBooksAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<List<Category>> GetAllWithBooksAsync()
    {
        return await _dbSet
            .Include(c => c.Books)
            .Where(c => !c.IsDeleted)
            .ToListAsync();
    }
    public async Task<(List<Category> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize)
    {
    var query = _dbSet
        .Include(c => c.Books)
        .Where(c => !c.IsDeleted);

    var totalCount = await query.CountAsync();

    var items = await query
        .OrderByDescending(c => c.CreatedDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (items, totalCount);
    }
}