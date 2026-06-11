using AutoMapper;
using LibraryManagement.Business.DTOs.Penalty;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Mappings;

public class PenaltyProfile : Profile
{
    public PenaltyProfile()
    {
        CreateMap<Penalty, PenaltyListDto>()
            .ForMember(dest => dest.BookTitle,
                       opt => opt.MapFrom(src => src.Loan.Book.Title))
            .ForMember(dest => dest.MemberFullName,
                       opt => opt.MapFrom(src => src.Loan.Member.FirstName + " " + src.Loan.Member.LastName));

        CreateMap<Penalty, PenaltyDetailDto>()
            .ForMember(dest => dest.BookTitle,
                       opt => opt.MapFrom(src => src.Loan.Book.Title))
            .ForMember(dest => dest.BookISBN,
                       opt => opt.MapFrom(src => src.Loan.Book.ISBN))
            .ForMember(dest => dest.MemberId,
                       opt => opt.MapFrom(src => src.Loan.MemberId))
            .ForMember(dest => dest.MemberFullName,
                       opt => opt.MapFrom(src => src.Loan.Member.FirstName + " " + src.Loan.Member.LastName))
            .ForMember(dest => dest.MemberEmail,
                       opt => opt.MapFrom(src => src.Loan.Member.Email));
    }
}