using System.ComponentModel.DataAnnotations.Schema;

namespace JustBlog.Models.Entities
{
    public class PostTagMap
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}