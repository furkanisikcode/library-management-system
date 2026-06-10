using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuthorListDto>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDetailDto>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
            return NotFound($"Id={id} olan yazar bulunamadı.");
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDetailDto>> Create([FromBody] AuthorCreateDto createDto)
    {
        var author = await _authorService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuthorDetailDto>> Update(int id, [FromBody] AuthorUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var author = await _authorService.UpdateAsync(updateDto);
        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _authorService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan yazar bulunamadı.");
        return NoContent();
    }
}