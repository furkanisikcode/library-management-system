using AutoMapper;
using LibraryManagement.Business.DTOs.Loan;
using LibraryManagement.Entities.Concrete;
using LibraryManagement.Entities.Enums;

namespace LibraryManagement.Business.Mappings;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<Loan, LoanListDto>()
            .ForMember(dest => dest.BookTitle,
                       opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.MemberFullName,
                       opt => opt.MapFrom(src => src.Member.FirstName + " " + src.Member.LastName))
            .ForMember(dest => dest.IsOverdue,
                       opt => opt.MapFrom(src =>
                           src.Status == LoanStatus.Active && DateTime.UtcNow > src.DueDate));

        CreateMap<Loan, LoanDetailDto>()
            .ForMember(dest => dest.BookTitle,
                       opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.BookISBN,
                       opt => opt.MapFrom(src => src.Book.ISBN))
            .ForMember(dest => dest.MemberFullName,
                       opt => opt.MapFrom(src => src.Member.FirstName + " " + src.Member.LastName))
            .ForMember(dest => dest.MemberEmail,
                       opt => opt.MapFrom(src => src.Member.Email))
            .ForMember(dest => dest.IsOverdue,
                       opt => opt.MapFrom(src =>
                           src.Status == LoanStatus.Active && DateTime.UtcNow > src.DueDate))
            .ForMember(dest => dest.DaysOverdue,
                       opt => opt.MapFrom(src =>
                           src.Status == LoanStatus.Active && DateTime.UtcNow > src.DueDate
                               ? (int)(DateTime.UtcNow - src.DueDate).TotalDays
                               : 0))
            .ForMember(dest => dest.TotalPenaltyAmount,
                       opt => opt.MapFrom(src =>
                           src.Penalties.Where(p => !p.IsDeleted).Sum(p => p.Amount)));
    }
}