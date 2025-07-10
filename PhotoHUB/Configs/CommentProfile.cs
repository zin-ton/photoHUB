using AutoMapper;

namespace PhotoHUB.configs;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<models.Comment, DTO.CommentDTO>()
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime));
    }
}