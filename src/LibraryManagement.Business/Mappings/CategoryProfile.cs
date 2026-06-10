using AutoMapper;
using LibraryManagement.Business.DTOs.Category;
using LibraryManagement.Entities.Concrete;

namespace LibraryManagement.Business.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryListDto>()
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<Category, CategoryDetailDto>()
            .ForMember(dest => dest.BookCount,
                       opt => opt.MapFrom(src => src.Books.Count(b => !b.IsDeleted)));

        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
    }
}