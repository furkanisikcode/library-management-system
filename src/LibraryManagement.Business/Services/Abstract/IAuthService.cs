using LibraryManagement.Business.DTOs.Auth;

namespace LibraryManagement.Business.Services.Abstract;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}