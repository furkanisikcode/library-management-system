using AutoMapper;
using LibraryManagement.Business.DTOs.Publisher;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Mappings;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Publisher, PublisherListDto>()
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<Publisher, PublisherDetailDto>()
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<PublisherCreateDto, Publisher>();
        CreateMap<PublisherUpdateDto, Publisher>();
    }
}