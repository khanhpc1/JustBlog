using JustBlog.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Models.Entities
{
    public class Tag 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public int Count { get; set; }
        public ICollection<PostTagMap> PostTagMaps { get; set; }
    }
}
