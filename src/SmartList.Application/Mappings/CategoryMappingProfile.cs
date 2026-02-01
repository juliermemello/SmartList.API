using AutoMapper;
using SmartList.Application.DTOs.Category;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, Category>();

        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.Products.Count));

        CreateMap<CategoryCreateRequest, Category>();

        CreateMap<CategoryUpdateRequest, Category>()
            .ReverseMap();

        CreateMap<CategoryUpdateRequest, CategoryCreateRequest>()
            .ReverseMap();
    }
}
