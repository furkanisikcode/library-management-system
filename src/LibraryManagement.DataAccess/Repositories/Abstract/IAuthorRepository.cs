using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetByIdWithBooksAsync(int id);
    Task<List<Author>> GetAllWithBooksAsync();

    Task<(List<Author> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize);
}