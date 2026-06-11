using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.Pagination;

namespace LibraryManagement.Business.Services.Abstract;

public interface IBookService
{
    Task<BookDetailDto?> GetByIdAsync(int id);
    Task<List<BookListDto>> GetAllAsync();
    Task<PagedResult<BookListDto>> GetPagedAsync(PaginationParams paginationParams);
    Task<BookDetailDto> CreateAsync(BookCreateDto createDto);
    Task<BookDetailDto> UpdateAsync(BookUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}