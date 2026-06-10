using LibraryManagement.Business.DTOs.Publisher;
using LibraryManagement.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost]
    public async Task<ActionResult<PublisherDetailDto>> Create([FromBody] PublisherCreateDto createDto)
    {
        var publisher = await _publisherService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = publisher.Id }, publisher);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PublisherDetailDto>> Update(int id, [FromBody] PublisherUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var publisher = await _publisherService.UpdateAsync(updateDto);
        return Ok(publisher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _publisherService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan yayınevi bulunamadı.");

        return NoContent();
    }
}