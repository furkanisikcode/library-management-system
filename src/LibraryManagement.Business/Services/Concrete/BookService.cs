using AutoMapper;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.Exceptions;       
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Business.Pagination;
using LibraryManagement.DataAccess.Repositories.Concrete;

namespace LibraryManagement.Business.Services.Concrete;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository; 
    //private readonly IRepository<Book> _bookRepository2;

    private readonly IRepository<Author> _authorRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public BookService(
        IBookRepository bookRepository,
        IRepository<Author> authorRepository,
        IRepository<Category> categoryRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BookDetailDto?> GetByIdAsync(int id)
    {
        //var a = new BookRepository();
        //var book = await a.GetByIdWithDetailsAsync(id);

        var book = await _bookRepository.GetByIdWithDetailsAsync(id);
        if (book == null) return null;
        return _mapper.Map<BookDetailDto>(book);
    }

    public async Task<List<BookListDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllWithDetailsAsync();
        return _mapper.Map<List<BookListDto>>(books);
    }
    public async Task<PagedResult<BookListDto>> GetPagedAsync(PaginationParams paginationParams)
    {
    var (books, totalCount) = await _bookRepository.GetPagedWithDetailsAsync(
        paginationParams.PageNumber,
        paginationParams.PageSize);

    var bookDtos = _mapper.Map<List<BookListDto>>(books);

    return new PagedResult<BookListDto>
    {
        Items = bookDtos,
        PageNumber = paginationParams.PageNumber,
        PageSize = paginationParams.PageSize,
        TotalCount = totalCount
    };
    }

    public async Task<BookDetailDto> CreateAsync(BookCreateDto createDto)
    {
        var book = _mapper.Map<Book>(createDto);

        var authors = await _authorRepository.FindAsync(a => createDto.AuthorIds.Contains(a.Id));
        foreach (var author in authors)
            book.Authors.Add(author);

        var categories = await _categoryRepository.FindAsync(c => createDto.CategoryIds.Contains(c.Id));
        foreach (var category in categories)
            book.Categories.Add(category);

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();

        var createdBook = await _bookRepository.GetByIdWithDetailsAsync(book.Id);
        return _mapper.Map<BookDetailDto>(createdBook);
    }

    public async Task<BookDetailDto> UpdateAsync(BookUpdateDto updateDto)
    {
        var book = await _bookRepository.GetByIdWithDetailsAsync(updateDto.Id);
        if (book == null)
            throw new NotFoundException($"Id={updateDto.Id} olan kitap bulunamadı.");

        _mapper.Map(updateDto, book);

        book.Authors.Clear();
        var authors = await _authorRepository.FindAsync(a => updateDto.AuthorIds.Contains(a.Id));
        foreach (var author in authors)
            book.Authors.Add(author);

        book.Categories.Clear();
        var categories = await _categoryRepository.FindAsync(c => updateDto.CategoryIds.Contains(c.Id));
        foreach (var category in categories)
            book.Categories.Add(category);

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();

        var updatedBook = await _bookRepository.GetByIdWithDetailsAsync(book.Id);
        return _mapper.Map<BookDetailDto>(updatedBook);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return false;

        _bookRepository.Delete(book);
        await _bookRepository.SaveChangesAsync();
        return true;
    }
}