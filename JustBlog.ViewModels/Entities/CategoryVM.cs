using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.ViewModels.EntityViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class SidebarCategoryVM
    {
        public string Name { get; set; }
    }
}