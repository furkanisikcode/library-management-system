using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.DTOs.Loan;

public class LoanDetailDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookISBN { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }
    public decimal TotalPenaltyAmount { get; set; }
    public DateTime CreatedDate { get; set; }
}