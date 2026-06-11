namespace LibraryManagement.Business.DTOs.Penalty;

public class PenaltyDetailDto
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookISBN { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedDate { get; set; }
}