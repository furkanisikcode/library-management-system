using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.DTOs.Loan;

public class LoanListDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberFullName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public bool IsOverdue { get; set; }
}