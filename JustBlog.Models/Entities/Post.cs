using JustBlog.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustBlog.Models.Entities
{
    public class Post
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

        public ICollection<PostTagMap> PostTagMaps { get; set; }
    }
}