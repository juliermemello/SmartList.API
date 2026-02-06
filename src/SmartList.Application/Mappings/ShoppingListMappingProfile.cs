using AutoMapper;
using SmartList.Application.DTOs.ShoppingList;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;

public class ShoppingListMappingProfile : Profile
{
    public ShoppingListMappingProfile()
    {
        CreateMap<ShoppingList, ShoppingList>();

        CreateMap<ShoppingList, ShoppingListResponse>()
            .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.Items.Count))
            .ForMember(dest => dest.CompletedProducts, opt => opt.MapFrom(src => src.Items.Count(p => p.IsChecked)));

        CreateMap<ShoppingListUpdateRequest, ShoppingList>()
            .ReverseMap();

        CreateMap<ShoppingListUpdateRequest, ShoppingListCreateRequest>()
            .ReverseMap();
    }
}
