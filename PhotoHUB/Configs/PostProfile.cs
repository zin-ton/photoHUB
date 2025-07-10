using AutoMapper;
using PhotoHUB.DTO;
using PhotoHUB.models;

namespace PhotoHUB.configs;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostPreviewDTO>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorLogin, opt => opt.MapFrom(src => src.User.Login));
        CreateMap<PostCreateDTO, Post>();
        CreateMap<PostUpdateDTO, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateTime, opt => opt.Ignore());
        CreateMap<Post, PostDTO>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dest => dest.AuthorLogin, opt => opt.MapFrom(src => src.User.Login));
    }
}