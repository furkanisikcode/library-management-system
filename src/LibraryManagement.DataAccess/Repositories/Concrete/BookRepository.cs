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
}