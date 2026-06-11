using AutoMapper;
using LibraryManagement.Business.DTOs.Publisher;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Business.Pagination;

namespace LibraryManagement.Business.Services.Concrete;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public PublisherService(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<PublisherDetailDto?> GetByIdAsync(int id)
    {
        var publisher = await _publisherRepository.GetByIdWithBooksAsync(id);
        if (publisher == null) return null;

        return _mapper.Map<PublisherDetailDto>(publisher);
    }

    public async Task<List<PublisherListDto>> GetAllAsync()
    {
        var publishers = await _publisherRepository.GetAllWithBooksAsync();
        return _mapper.Map<List<PublisherListDto>>(publishers);
    }

    public async Task<PublisherDetailDto> CreateAsync(PublisherCreateDto createDto)
    {
        var publisher = _mapper.Map<Publisher>(createDto);
        await _publisherRepository.AddAsync(publisher);
        await _publisherRepository.SaveChangesAsync();

        return _mapper.Map<PublisherDetailDto>(publisher);
    }

    public async Task<PublisherDetailDto> UpdateAsync(PublisherUpdateDto updateDto)
    {
        var publisher = await _publisherRepository.GetByIdAsync(updateDto.Id);
        if (publisher == null)
            throw new Exception($"Id={updateDto.Id} olan yayınevi bulunamadı.");

        _mapper.Map(updateDto, publisher);
        _publisherRepository.Update(publisher);
        await _publisherRepository.SaveChangesAsync();

        return _mapper.Map<PublisherDetailDto>(publisher);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher == null) return false;

        _publisherRepository.Delete(publisher);
        await _publisherRepository.SaveChangesAsync();
        return true;
    }
    public async Task<PagedResult<PublisherListDto>> GetPagedAsync(PaginationParams paginationParams)
    {
    var (publishers, totalCount) = await _publisherRepository.GetPagedWithBooksAsync(
        paginationParams.PageNumber,
        paginationParams.PageSize);

    return new PagedResult<PublisherListDto>
    {
        Items = _mapper.Map<List<PublisherListDto>>(publishers),
        PageNumber = paginationParams.PageNumber,
        PageSize = paginationParams.PageSize,
        TotalCount = totalCount
    };
    }
}