using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IPublisherRepository : IRepository<Publisher>
{
    Task<Publisher?> GetByIdWithBooksAsync(int id);
    Task<List<Publisher>> GetAllWithBooksAsync();
    Task<(List<Publisher> Items, int TotalCount)> GetPagedWithBooksAsync(int pageNumber, int pageSize);
}