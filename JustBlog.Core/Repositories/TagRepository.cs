using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Core.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly JustBlogDbContext _db;

        public TagRepository(JustBlogDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Tag entity)
        {
            await _db.Tags.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Tag entity)
        {
            _db.Tags.Remove(entity);
            return await Save();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> Find(int id)
        {
            var tag = await _db.Tags.FindAsync(id);
            return tag;
        }

        public async Task<ICollection<Tag>> GetAll()
        {
            var categories = await _db.Tags.ToListAsync();
            return categories;
        }

        public async Task<Tag> GetById(int id)
        {
            var tag = await _db.Tags.FindAsync();
            return tag;
        }

        public Task<Tag> GetTagByUrlSlug(string urlSlug)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExists(int id)
        {
            var exists = await _db.Tags.AnyAsync(c => c.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Tag entity)
        {
            _db.Tags.Update(entity);

            return await Save();
        }
    }
}