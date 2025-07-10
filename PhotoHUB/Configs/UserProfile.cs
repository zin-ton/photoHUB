using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.configs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        CreateMap<UserLoginDTO, User>();
        CreateMap<User, GetUserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
    }
}