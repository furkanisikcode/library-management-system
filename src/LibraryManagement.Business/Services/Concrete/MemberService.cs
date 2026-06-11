using AutoMapper;
using LibraryManagement.Business.DTOs.Member;
using LibraryManagement.Business.Exceptions;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Business.Pagination;

namespace LibraryManagement.Business.Services.Concrete;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<MemberDetailDto?> GetByIdAsync(int id)
    {
        var member = await _memberRepository.GetByIdWithLoansAsync(id);
        if (member == null) return null;
        return _mapper.Map<MemberDetailDto>(member);
    }

    public async Task<List<MemberListDto>> GetAllAsync()
    {
        var members = await _memberRepository.GetAllWithLoansAsync();
        return _mapper.Map<List<MemberListDto>>(members);
    }

    public async Task<MemberDetailDto> CreateAsync(MemberCreateDto createDto)
    {
        var existingMember = await _memberRepository.GetByEmailAsync(createDto.Email);
        if (existingMember != null)
            throw new ConflictException($"'{createDto.Email}' e-posta adresi zaten kayıtlı.");

        var member = _mapper.Map<Member>(createDto);
        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        return _mapper.Map<MemberDetailDto>(member);
    }

    public async Task<MemberDetailDto> UpdateAsync(MemberUpdateDto updateDto)
    {
        var member = await _memberRepository.GetByIdAsync(updateDto.Id);
        if (member == null)
            throw new NotFoundException($"Id={updateDto.Id} olan üye bulunamadı.");

        if (member.Email != updateDto.Email)
        {
            var existingMember = await _memberRepository.GetByEmailAsync(updateDto.Email);
            if (existingMember != null && existingMember.Id != updateDto.Id)
                throw new ConflictException($"'{updateDto.Email}' e-posta adresi başka bir üyede kayıtlı.");
        }

        _mapper.Map(updateDto, member);
        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync();

        return _mapper.Map<MemberDetailDto>(member);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return false;

        _memberRepository.Delete(member);
        await _memberRepository.SaveChangesAsync();
        return true;
    }
    public async Task<PagedResult<MemberListDto>> GetPagedAsync(PaginationParams paginationParams)
    {
    var (members, totalCount) = await _memberRepository.GetPagedWithLoansAsync(
        paginationParams.PageNumber,
        paginationParams.PageSize);

    return new PagedResult<MemberListDto>
    {
        Items = _mapper.Map<List<MemberListDto>>(members),
        PageNumber = paginationParams.PageNumber,
        PageSize = paginationParams.PageSize,
        TotalCount = totalCount
    };
}
}