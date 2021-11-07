using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.ViewModels.EntityViewModels
{
    public class TagVM
    {
        public int Id { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public ICollection<PostTagMap> PostTagMaps { get; set; }
    }
}