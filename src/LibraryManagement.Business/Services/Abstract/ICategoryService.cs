using LibraryManagement.Business.DTOs.Category;

namespace LibraryManagement.Business.Services.Abstract;

public interface ICategoryService
{
    Task<CategoryDetailDto?> GetByIdAsync(int id);
    Task<List<CategoryListDto>> GetAllAsync();
    Task<CategoryDetailDto> CreateAsync(CategoryCreateDto createDto);
    Task<CategoryDetailDto> UpdateAsync(CategoryUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}