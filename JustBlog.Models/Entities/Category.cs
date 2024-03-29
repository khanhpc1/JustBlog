﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustBlog.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}