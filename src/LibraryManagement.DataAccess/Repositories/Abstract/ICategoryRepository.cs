using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdWithBooksAsync(int id);
    Task<List<Category>> GetAllWithBooksAsync();
}