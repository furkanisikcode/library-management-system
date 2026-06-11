using LibraryManagement.Business.DTOs.Member;
using LibraryManagement.Business.Pagination;
namespace LibraryManagement.Business.Services.Abstract;

public interface IMemberService
{
    Task<MemberDetailDto?> GetByIdAsync(int id);
    Task<List<MemberListDto>> GetAllAsync();
    Task<MemberDetailDto> CreateAsync(MemberCreateDto createDto);
    Task<MemberDetailDto> UpdateAsync(MemberUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResult<MemberListDto>> GetPagedAsync(PaginationParams paginationParams);
}