using LibraryManagement.Business.DTOs.Member;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MemberListDto>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDetailDto>> GetById(int id)
    {
        var member = await _memberService.GetByIdAsync(id);
        if (member == null)
            return NotFound($"Id={id} olan üye bulunamadı.");
        return Ok(member);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<MemberListDto>>> GetPaged([FromQuery] PaginationParams paginationParams)
    {
        var result = await _memberService.GetPagedAsync(paginationParams);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDetailDto>> Create([FromBody] MemberCreateDto createDto)
    {
        var member = await _memberService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDetailDto>> Update(int id, [FromBody] MemberUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest("URL'deki ID ile body'deki ID eşleşmiyor.");

        var member = await _memberService.UpdateAsync(updateDto);
        return Ok(member);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _memberService.DeleteAsync(id);
        if (!result)
            return NotFound($"Id={id} olan üye bulunamadı.");
        return NoContent();
    }
}