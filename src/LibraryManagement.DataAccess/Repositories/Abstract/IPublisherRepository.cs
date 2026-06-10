using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.DataAccess.Repositories.Abstract;

public interface IPublisherRepository : IRepository<Publisher>
{
    Task<Publisher?> GetByIdWithBooksAsync(int id);
    Task<List<Publisher>> GetAllWithBooksAsync();
}