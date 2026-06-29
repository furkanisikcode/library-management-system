using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.DTOs.Auth;

public class AuthResponseDto
{
    public int MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public MemberRoleType Role { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime TokenExpiresAt { get; set; }
}