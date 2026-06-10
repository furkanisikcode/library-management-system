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
}