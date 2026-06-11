using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Business.Pagination;

namespace LibraryManagement.Business.Services.Abstract;

public interface IAuthorService
{
    Task<AuthorDetailDto?> GetByIdAsync(int id);
    Task<List<AuthorListDto>> GetAllAsync();
    Task<AuthorDetailDto> CreateAsync(AuthorCreateDto createDto);
    Task<AuthorDetailDto> UpdateAsync(AuthorUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResult<AuthorListDto>> GetPagedAsync(PaginationParams paginationParams);
}