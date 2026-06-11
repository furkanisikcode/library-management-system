using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Pagination;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<List<BookListDto>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<BookListDto>>> GetPaged([FromQuery] PaginationParams paginationParams)
    {   
    var result = await _bookService.GetPagedAsync(paginationParams);
    return Ok(result);
    }

    // GET: api/books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailDto>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
            return NotFound($"Id={id} olan kitap bulunamadı.");

        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookDetailDto>> Create([FromBody] BookCreateDto createDto)
    {
        var book = await _bookService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT: api/books/5
    [HttpPut("{id}")]
    public async Task<ActionResult<BookDetailDto>> Update(int id, [FromBody] BookUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var book = await _bookService.UpdateAsync(updateDto);
        return Ok(book);
    }

    // DELETE: api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan kitap bulunamadı.");

        return NoContent();
    }
}