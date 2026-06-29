using LibraryManagement.Business.DTOs.Category;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryListDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDetailDto>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Id={id} olan kategori bulunamadı.");
        return Ok(category);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<CategoryListDto>>> GetPaged([FromQuery] PaginationParams paginationParams)
    {
        var result = await _categoryService.GetPagedAsync(paginationParams);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<ActionResult<CategoryDetailDto>> Create([FromBody] CategoryCreateDto createDto)
    {
        var category = await _categoryService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<ActionResult<CategoryDetailDto>> Update(int id, [FromBody] CategoryUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var category = await _categoryService.UpdateAsync(updateDto);
        return Ok(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan kategori bulunamadı.");
        return NoContent();
    }
}