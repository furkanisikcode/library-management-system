using LibraryManagement.Business.DTOs.Publisher;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PublisherListDto>>> GetAll()
    {
        var publishers = await _publisherService.GetAllAsync();
        return Ok(publishers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublisherDetailDto>> GetById(int id)
    {
        var publisher = await _publisherService.GetByIdAsync(id);
        if (publisher == null)
            return NotFound($"Id={id} olan yayınevi bulunamadı.");

        return Ok(publisher);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<PublisherListDto>>> GetPaged([FromQuery] PaginationParams paginationParams)
    {
        var result = await _publisherService.GetPagedAsync(paginationParams);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<ActionResult<PublisherDetailDto>> Create([FromBody] PublisherCreateDto createDto)
    {
        var publisher = await _publisherService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = publisher.Id }, publisher);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<ActionResult<PublisherDetailDto>> Update(int id, [FromBody] PublisherUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var publisher = await _publisherService.UpdateAsync(updateDto);
        return Ok(publisher);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _publisherService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan yayınevi bulunamadı.");

        return NoContent();
    }
}