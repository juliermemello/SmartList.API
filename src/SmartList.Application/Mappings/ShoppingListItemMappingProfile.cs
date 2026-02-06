using AutoMapper;
using SmartList.Application.DTOs.ShoppingListItem;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;

public class ShoppingListItemMappingProfile : Profile
{
    public ShoppingListItemMappingProfile()
    {
        CreateMap<ListItem, ListItem>();

        CreateMap<ListItem, ShoppingListItemResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
            .ForMember(
                dest => dest.Product,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name ?? string.Empty : string.Empty)
            )
            .ForMember(
                dest => dest.CategoryId,
                opt => opt.MapFrom(src =>
                    src.Product != null && src.Product.Category != null
                        ? src.Product.Category.Id
                        : 0
                )
            )
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src =>
                    src.Product != null && src.Product.Category != null
                        ? src.Product.Category.Name ?? string.Empty
                        : string.Empty
                )
            );

        CreateMap<ShoppingListItemUpdateRequest, ListItem>()
            .ReverseMap();

        CreateMap<ShoppingListItemUpdateRequest, ShoppingListItemCreateRequest>()
            .ReverseMap();
    }
}
