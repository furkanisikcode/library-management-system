using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdWithBooksAsync(int id);
    Task<List<Category>> GetAllWithBooksAsync();
    Task<(List<Category> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize);
}