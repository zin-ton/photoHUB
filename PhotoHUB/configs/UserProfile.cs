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
        CreateMap<User, GetUserDTO>();
    }
}