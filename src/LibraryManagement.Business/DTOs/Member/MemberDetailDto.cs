using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.DTOs.Member;

public class MemberDetailDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }
    public MemberRoleType Role { get; set; }
    public int ActiveLoanCount { get; set; }
    public int TotalLoanCount { get; set; }
    public DateTime CreatedDate { get; set; }
}