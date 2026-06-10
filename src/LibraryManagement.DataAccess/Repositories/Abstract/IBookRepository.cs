using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetByIdWithDetailsAsync(int id);
    Task<List<Book>> GetAllWithDetailsAsync();
}