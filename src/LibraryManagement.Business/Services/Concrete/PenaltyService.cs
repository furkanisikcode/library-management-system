using AutoMapper;
using LibraryManagement.Business.DTOs.Penalty;
using LibraryManagement.Business.Exceptions;
using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.DataAccess.Repositories.Abstract;

namespace LibraryManagement.Business.Services.Concrete;

public class PenaltyService : IPenaltyService
{
    private readonly IPenaltyRepository _penaltyRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public PenaltyService(
        IPenaltyRepository penaltyRepository,
        IMemberRepository memberRepository,
        IMapper mapper)
    {
        _penaltyRepository = penaltyRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<PenaltyDetailDto?> GetByIdAsync(int id)
    {
        var penalty = await _penaltyRepository.GetByIdWithDetailsAsync(id);
        if (penalty == null) return null;
        return _mapper.Map<PenaltyDetailDto>(penalty);
    }

    public async Task<List<PenaltyListDto>> GetAllAsync()
    {
        var penalties = await _penaltyRepository.GetAllWithDetailsAsync();
        return _mapper.Map<List<PenaltyListDto>>(penalties);
    }

    public async Task<MemberPenaltySummaryDto?> GetMemberSummaryAsync(int memberId)
    {
        // Önce üyenin var olduğunu kontrol et
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            throw new NotFoundException($"Id={memberId} olan üye bulunamadı.");

        var penalties = await _penaltyRepository.GetByMemberIdAsync(memberId);
        var penaltyDtos = _mapper.Map<List<PenaltyListDto>>(penalties);

        return new MemberPenaltySummaryDto
        {
            MemberId = member.Id,
            MemberFullName = $"{member.FirstName} {member.LastName}",
            MemberEmail = member.Email,
            TotalPenaltyAmount = penalties.Sum(p => p.Amount),
            UnpaidAmount = penalties.Where(p => !p.IsPaid).Sum(p => p.Amount),
            PaidAmount = penalties.Where(p => p.IsPaid).Sum(p => p.Amount),
            TotalPenaltyCount = penalties.Count,
            UnpaidCount = penalties.Count(p => !p.IsPaid),
            Penalties = penaltyDtos
        };
    }

    public async Task<PenaltyDetailDto> PayPenaltyAsync(int penaltyId)
    {
        var penalty = await _penaltyRepository.GetByIdWithDetailsAsync(penaltyId);
        if (penalty == null)
            throw new NotFoundException($"Id={penaltyId} olan ceza bulunamadı.");

        if (penalty.IsPaid)
            throw new BusinessRuleException(
                $"Bu ceza {penalty.PaidDate:dd.MM.yyyy} tarihinde zaten ödenmiş.");

        penalty.IsPaid = true;
        penalty.PaidDate = DateTime.UtcNow;
        _penaltyRepository.Update(penalty);
        await _penaltyRepository.SaveChangesAsync();

        return _mapper.Map<PenaltyDetailDto>(penalty);
    }
}