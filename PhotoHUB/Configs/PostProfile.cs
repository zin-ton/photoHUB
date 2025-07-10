using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.configs;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostPreviewDTO>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id));
        CreateMap<PostCreateDTO, Post>();
        CreateMap<PostUpdateDTO, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateTime, opt => opt.Ignore());
    }
}