using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;

namespace JustBlog.ViewModels.EntityViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlSlug { get; set; }
        public string ShortDescription { get; set; }

        public string PostContent { get; set; }
        public bool Published { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime Modified { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<PostTagMap> PostTagMaps { get; set; }

        public int[] TagIds { get; set; }
    }

    public class ListPostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string UrlSlug { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PostedOn { get; set; }
    }

    public class DetailPostVM
    {
        
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string PostContent { get; set; }
        public DateTime PostedOn { get; set; }
    }

    public class LastestPostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PostedOn { get; set; }

    }
}