using AutoMapper;
using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Services.Concrete;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<AuthorDetailDto?> GetByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdWithBooksAsync(id);
        if (author == null) return null;
        return _mapper.Map<AuthorDetailDto>(author);
    }

    public async Task<List<AuthorListDto>> GetAllAsync()
    {
        var authors = await _authorRepository.GetAllWithBooksAsync();
        return _mapper.Map<List<AuthorListDto>>(authors);
    }

    public async Task<AuthorDetailDto> CreateAsync(AuthorCreateDto createDto)
    {
        var author = _mapper.Map<Author>(createDto);
        await _authorRepository.AddAsync(author);
        await _authorRepository.SaveChangesAsync();
        return _mapper.Map<AuthorDetailDto>(author);
    }

    public async Task<AuthorDetailDto> UpdateAsync(AuthorUpdateDto updateDto)
    {
        var author = await _authorRepository.GetByIdAsync(updateDto.Id);
        if (author == null)
            throw new Exception($"Id={updateDto.Id} olan yazar bulunamadı.");

        _mapper.Map(updateDto, author);
        _authorRepository.Update(author);
        await _authorRepository.SaveChangesAsync();
        return _mapper.Map<AuthorDetailDto>(author);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null) return false;

        _authorRepository.Delete(author);
        await _authorRepository.SaveChangesAsync();
        return true;
    }
}