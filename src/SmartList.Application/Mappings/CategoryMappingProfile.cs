using AutoMapper;
using SmartList.Application.DTOs.Category;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryResponse>();

        CreateMap<CategoryCreateRequest, Category>();

        CreateMap<CategoryUpdateRequest, Category>()
            .ReverseMap();

        CreateMap<CategoryUpdateRequest, CategoryCreateRequest>()
            .ReverseMap();
    }
}
