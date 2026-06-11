using LibraryManagement.Business.DTOs.Category;
using LibraryManagement.Business.Pagination;
namespace LibraryManagement.Business.Services.Abstract;

public interface ICategoryService
{
    Task<CategoryDetailDto?> GetByIdAsync(int id);
    Task<List<CategoryListDto>> GetAllAsync();
    Task<CategoryDetailDto> CreateAsync(CategoryCreateDto createDto);
    Task<CategoryDetailDto> UpdateAsync(CategoryUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResult<CategoryListDto>> GetPagedAsync(PaginationParams paginationParams);
}