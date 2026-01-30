using AutoMapper;
using SmartList.Application.DTOs.Product;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductResponse>();

        CreateMap<ProductCreateRequest, Product>();

        CreateMap<ProductUpdateRequest, Product>()
            .ReverseMap();

        CreateMap<ProductUpdateRequest, ProductCreateRequest>()
            .ReverseMap();
    }
}
