namespace LibraryManagement.Business.DTOs.Penalty;

public class MemberPenaltySummaryDto
{
    public int MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public decimal TotalPenaltyAmount { get; set; }
    public decimal UnpaidAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public int TotalPenaltyCount { get; set; }
    public int UnpaidCount { get; set; }
    public List<PenaltyListDto> Penalties { get; set; } = new();
}