using AutoMapper;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        // Entity → DTO (okuma)
        CreateMap<Book, BookListDto>()
            .ForMember(dest => dest.PublisherName,
                       opt => opt.MapFrom(src => src.Publisher.Name));

        CreateMap<Book, BookDetailDto>()
            .ForMember(dest => dest.PublisherName,
                       opt => opt.MapFrom(src => src.Publisher.Name))
            .ForMember(dest => dest.AuthorNames,
                       opt => opt.MapFrom(src => src.Authors.Select(a => a.FirstName + " " + a.LastName)))
            .ForMember(dest => dest.CategoryNames,
                       opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

        // DTO → Entity (yazma)
        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore());

        CreateMap<BookUpdateDto, Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
    }
}