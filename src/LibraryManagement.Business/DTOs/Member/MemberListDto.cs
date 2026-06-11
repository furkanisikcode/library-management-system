using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.DTOs.Member;

public class MemberListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public MemberRoleType Role { get; set; }
    public bool IsActive { get; set; }
    public int ActiveLoanCount { get; set; }
}