using AutoMapper;
using LibraryManagement.Business.DTOs.Category;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDetailDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdWithBooksAsync(id);
        if (category == null) return null;
        return _mapper.Map<CategoryDetailDto>(category);
    }

    public async Task<List<CategoryListDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllWithBooksAsync();
        return _mapper.Map<List<CategoryListDto>>(categories);
    }

    public async Task<CategoryDetailDto> CreateAsync(CategoryCreateDto createDto)
    {
        var category = _mapper.Map<Category>(createDto);
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
        return _mapper.Map<CategoryDetailDto>(category);
    }

    public async Task<CategoryDetailDto> UpdateAsync(CategoryUpdateDto updateDto)
    {
        var category = await _categoryRepository.GetByIdAsync(updateDto.Id);
        if (category == null)
            throw new Exception($"Id={updateDto.Id} olan kategori bulunamadı.");

        _mapper.Map(updateDto, category);
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();
        return _mapper.Map<CategoryDetailDto>(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return false;

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
        return true;
    }
}