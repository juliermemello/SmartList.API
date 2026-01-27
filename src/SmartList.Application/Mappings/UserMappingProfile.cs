using AutoMapper;
using SmartList.Application.DTOs.User;
using SmartList.Domain.Entity;

namespace SmartList.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.TotalLists, opt => opt.MapFrom(src => src.Lists.Count))
            .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.Products.Count));

        CreateMap<UserCreateRequest, User>();
    }
}
