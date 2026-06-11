namespace LibraryManagement.Business.DTOs.Penalty;

public class PenaltyListDto
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberFullName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedDate { get; set; }
}