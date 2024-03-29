﻿using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Core.Contracts
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        Task<Tag> GetTagByUrlSlug(string urlSlug);
    }
}
