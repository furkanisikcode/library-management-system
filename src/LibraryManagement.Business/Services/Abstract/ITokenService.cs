using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Services.Abstract;

public interface ITokenService
{
    string GenerateToken(Member member);
}