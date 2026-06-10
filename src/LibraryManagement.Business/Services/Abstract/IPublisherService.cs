using LibraryManagement.Business.DTOs.Publisher;

namespace LibraryManagement.Business.Services.Abstract;

public interface IPublisherService
{
    Task<PublisherDetailDto?> GetByIdAsync(int id);
    Task<List<PublisherListDto>> GetAllAsync();
    Task<PublisherDetailDto> CreateAsync(PublisherCreateDto createDto);
    Task<PublisherDetailDto> UpdateAsync(PublisherUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}