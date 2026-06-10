using LibraryManagement.Entities.Common;

namespace LibraryManagement.Entities.Concrete;

public class Penalty : BaseEntity
{
    public int LoanId { get; set; }
    public Loan Loan { get; set; } = null!;

    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }
}