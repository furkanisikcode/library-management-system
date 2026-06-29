using LibraryManagement.Entities.Common;
using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Entities.Concrete;

public class Member : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
     public string PasswordHash { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Üye rolü (enum)
    public MemberRoleType Role { get; set; } = MemberRoleType.Member;

    // Navigation Property: Bir üyenin birçok ödünç işlemi olabilir
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    
}