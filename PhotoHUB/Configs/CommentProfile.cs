using AutoMapper;
using PhotoHUB.DTO;

namespace PhotoHUB.configs;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<models.Comment, DTO.CommentDTO>()
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dest => dest.AuthorS3Key, opt => opt.MapFrom(src => src.User.S3Key))
            .ForMember(dest => dest.AuthorLogin, opt => opt.MapFrom(src => src.User.Login));
        CreateMap<DTO.CreateCommentDTO, models.Comment>()
            .ForMember(dest => dest.ReplyToId, opt => opt.MapFrom(src => Guid.Parse(src.ReplyToId)));
        CreateMap<UpdateCommentDTO, models.Comment>();
    }
}