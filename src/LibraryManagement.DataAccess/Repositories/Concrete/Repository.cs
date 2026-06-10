using System.Linq.Expressions;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories.Concrete;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly LibraryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(e => !e.IsDeleted).Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        // Soft delete: gerçekten silmiyoruz, sadece bayrağı işaretliyoruz
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}