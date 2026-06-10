using AutoMapper;
using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Mappings;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorListDto>()
            .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<Author, AuthorDetailDto>()
            .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<AuthorCreateDto, Author>();
        CreateMap<AuthorUpdateDto, Author>();
    }
}