using LibraryManagement.Entities.Common;
using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Entities.Concrete;

public class Loan : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public LoanStatus Status { get; set; } = LoanStatus.Active;

    // Navigation Property: Bir ödünç işleminin birden fazla cezası olabilir
    public ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
}