using AutoMapper;
using LibraryManagement.Business.DTOs.Member;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.Mappings;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<Member, MemberListDto>()
            .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.ActiveLoanCount,
                       opt => opt.MapFrom(src => src.Loans.Count(l => !l.IsDeleted && l.Status == LoanStatus.Active)));

        CreateMap<Member, MemberDetailDto>()
            .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.ActiveLoanCount,
                       opt => opt.MapFrom(src => src.Loans.Count(l => !l.IsDeleted && l.Status == LoanStatus.Active)))
            .ForMember(dest => dest.TotalLoanCount,
                       opt => opt.MapFrom(src => src.Loans.Count(l => !l.IsDeleted)));

        CreateMap<MemberCreateDto, Member>();
        CreateMap<MemberUpdateDto, Member>();
    }
}