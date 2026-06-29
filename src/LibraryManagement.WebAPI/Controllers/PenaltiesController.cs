using LibraryManagement.Business.DTOs.Penalty;
using LibraryManagement.Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PenaltiesController : ControllerBase
{
    private readonly IPenaltyService _penaltyService;

    public PenaltiesController(IPenaltyService penaltyService)
    {
        _penaltyService = penaltyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PenaltyListDto>>> GetAll()
    {
        var penalties = await _penaltyService.GetAllAsync();
        return Ok(penalties);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PenaltyDetailDto>> GetById(int id)
    {
        var penalty = await _penaltyService.GetByIdAsync(id);
        if (penalty == null)
            return NotFound($"Id={id} olan ceza bulunamadı.");
        return Ok(penalty);
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<MemberPenaltySummaryDto>> GetMemberSummary(int memberId)
    {
        var summary = await _penaltyService.GetMemberSummaryAsync(memberId);
        return Ok(summary);
    }

    [HttpPost("{id}/pay")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<ActionResult<PenaltyDetailDto>> Pay(int id)
    {
        var penalty = await _penaltyService.PayPenaltyAsync(id);
        return Ok(penalty);
    }
}