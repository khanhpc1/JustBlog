using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JustBlog.Core.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(JustBlogDbContext context) : base(context)
        {
        }

        public Task<Tag> GetTagByUrlSlug(string urlSlug)
        {
            throw new NotImplementedException();
        }
    }
}