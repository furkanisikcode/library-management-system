using LibraryManagement.Business.DTOs.Loan;
using LibraryManagement.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LoanListDto>>> GetAll()
    {
        var loans = await _loanService.GetAllAsync();
        return Ok(loans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LoanDetailDto>> GetById(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null)
            return NotFound($"Id={id} olan ödünç işlemi bulunamadı.");
        return Ok(loan);
    }

    [HttpPost("borrow")]
    public async Task<ActionResult<LoanDetailDto>> Borrow([FromBody] LoanCreateDto createDto)
    {
        var loan = await _loanService.BorrowBookAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPost("return")]
    public async Task<ActionResult<LoanDetailDto>> Return([FromBody] LoanReturnDto returnDto)
    {
        var loan = await _loanService.ReturnBookAsync(returnDto);
        return Ok(loan);
    }
}