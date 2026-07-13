using LibraryManagement.Business.DTOs.Auth;
using LibraryManagement.Business.Exceptions;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Settings;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Entities.Enums;
using Microsoft.Extensions.Options;
using LibraryManagement.Common.Email;
using LibraryManagement.Common.Sms;

namespace LibraryManagement.Business.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public AuthService(
        IMemberRepository memberRepository,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtSettings,
        IEmailService emailService,
        ISmsService smsService)
    {
        _memberRepository = memberRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _emailService = emailService;
        _smsService = smsService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // 1. Email zaten kayıtlı mı?
        var existingMember = await _memberRepository.GetByEmailAsync(registerDto.Email);
        if (existingMember != null)
            throw new ConflictException($"'{registerDto.Email}' e-posta adresi zaten kayıtlı.");

        // 2. Parolayı hashle
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // 3. Yeni üyeyi oluştur (her zaman 'Member' rolü ile)
        var member = new Member
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PhoneNumber = registerDto.PhoneNumber,
            Address = registerDto.Address,
            Role = MemberRoleType.Member,
            IsActive = true,
            MembershipDate = DateTime.UtcNow
        };

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        // 4. Hoş geldin e-postası gönder (simülasyon → log)
        await _emailService.SendAsync(
            member.Email,
            "Kütüphaneye Hoş Geldiniz!",
            $"Merhaba {member.FirstName}, üyeliğiniz başarıyla oluşturuldu.");

        // 5. Hoş geldin SMS'i gönder (simülasyon → log)
        if (!string.IsNullOrWhiteSpace(member.PhoneNumber))
        {
            await _smsService.SendAsync(
                member.PhoneNumber,
                $"Merhaba {member.FirstName}, kütüphane üyeliğiniz aktif edildi.");
        }

        // 6. Token üret ve cevabı döndür
        return BuildAuthResponse(member);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // 1. Email ile üyeyi bul
        var member = await _memberRepository.GetByEmailAsync(loginDto.Email);
        if (member == null)
            throw new BusinessRuleException("E-posta veya parola hatalı.");

        // 2. Üye aktif mi?
        if (!member.IsActive)
            throw new BusinessRuleException("Hesabınız aktif değil. Yöneticinizle iletişime geçin.");

        // 3. Parolayı doğrula
        var passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, member.PasswordHash);
        if (!passwordValid)
            throw new BusinessRuleException("E-posta veya parola hatalı.");

        // 4. Token üret ve cevabı döndür
        return BuildAuthResponse(member);
    }

    private AuthResponseDto BuildAuthResponse(Member member)
    {
        var token = _tokenService.GenerateToken(member);
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        return new AuthResponseDto
        {
            MemberId = member.Id,
            FullName = $"{member.FirstName} {member.LastName}",
            Email = member.Email,
            Role = member.Role,
            Token = token,
            TokenExpiresAt = expiresAt
        };
    }
}