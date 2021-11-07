using AutoMapper;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;

namespace JustBlog.Web.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Post, PostVM>().ReverseMap();
            CreateMap<Post, ListPostVM>().ReverseMap();
            CreateMap<Tag, TagVM>().ReverseMap(); 
            CreateMap<PostTagMap, PostTagMapVM>().ReverseMap();
        }
    }
}