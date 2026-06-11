using LibraryManagement.Business.DTOs.Penalty;

namespace LibraryManagement.Business.Services.Abstract;

public interface IPenaltyService
{
    Task<PenaltyDetailDto?> GetByIdAsync(int id);
    Task<List<PenaltyListDto>> GetAllAsync();
    Task<MemberPenaltySummaryDto?> GetMemberSummaryAsync(int memberId);
    Task<PenaltyDetailDto> PayPenaltyAsync(int penaltyId);
}