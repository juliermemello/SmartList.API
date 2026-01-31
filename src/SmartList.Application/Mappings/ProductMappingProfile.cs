using AutoMapper;
using SmartList.Application.DTOs.Product;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, Product>();

        CreateMap<Product, ProductResponse>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name ?? string.Empty : string.Empty)
            );

        CreateMap<ProductCreateRequest, Product>();

        CreateMap<ProductUpdateRequest, Product>()
            .ReverseMap();

        CreateMap<ProductUpdateRequest, ProductCreateRequest>()
            .ReverseMap();
    }
}
