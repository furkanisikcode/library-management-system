using LibraryManagement.Business.DTOs.Book;

namespace LibraryManagement.Business.Services.Abstract;

public interface IBookService
{
    Task<BookDetailDto?> GetByIdAsync(int id);
    Task<List<BookListDto>> GetAllAsync();
    Task<BookDetailDto> CreateAsync(BookCreateDto createDto);
    Task<BookDetailDto> UpdateAsync(BookUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}