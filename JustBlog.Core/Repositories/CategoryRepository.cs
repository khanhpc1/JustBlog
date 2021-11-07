using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly JustBlogDbContext _db;
        public CategoryRepository(JustBlogDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Category entity)
        {
            await _db.Categories.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Category entity)
        {
            _db.Categories.Remove(entity);
            return await Save();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Find(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            return category;
        }

        public async Task<ICollection<Category>> GetAll()
        {
            var categories = await _db.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _db.Categories.FindAsync();
            return category;
        }

        public async Task<bool> IsExists(int id)
        {
            var exists = await _db.Categories.AnyAsync(c => c.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Category entity)
        {
            _db.Categories.Update(entity);

            return await Save();
        }
    }
}
