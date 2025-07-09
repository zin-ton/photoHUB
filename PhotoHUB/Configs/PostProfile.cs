using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.configs;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostPreviewDTO>();
        CreateMap<PostCreateDTO, Post>();
        CreateMap<PostUpdateDTO, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateTime, opt => opt.Ignore());

    }
}