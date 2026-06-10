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
}