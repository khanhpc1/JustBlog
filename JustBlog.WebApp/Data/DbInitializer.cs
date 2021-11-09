using JustBlog.Core;
using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Data
{
    public static class DbInitializer
    {
        public static void Initializer(JustBlogDbContext context)
        {
            //EnsureCreate is used to automatically create the database
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category { Id = 1, Name = "Category 1", UrlSlug = "http://sample1.com", Description = "Description1" },
                new Category { Id = 2, Name = "Category 2", UrlSlug = "http://sample2.com", Description = "Description2" },
                new Category { Id = 3, Name = "Category 3", UrlSlug = "http://sample3.com", Description = "Description3" },
                new Category { Id = 4, Name = "Category 4", UrlSlug = "http://sample4.com", Description = "Description4" },
                new Category { Id = 5, Name = "Category 5", UrlSlug = "http://sample5.com", Description = "Description5" },
                new Category { Id = 6, Name = "Category 6", UrlSlug = "http://sample6.com", Description = "Description6" },
                new Category { Id = 7, Name = "Category 7", UrlSlug = "http://sample7.com", Description = "Description7" },
                new Category { Id = 8, Name = "Category 8", UrlSlug = "http://sample8.com", Description = "Description8" },
                new Category { Id = 9, Name = "Category 9", UrlSlug = "http://sample9.com", Description = "Description9" },
                new Category { Id = 10, Name = "Category 10", UrlSlug = "http://sample20.com", Description = "Description10" }

            };

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();

            
            var posts = new Post[]
           {
                new Post { Id = 1, Title = " Post 1", ShortDescription = "short 1", PostContent = "Content1", UrlSlug = "Post 1 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 2, Title = " Post 2", ShortDescription = "short 2", PostContent = "Content2", UrlSlug = "Post 2 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 02), Modified = new DateTime(2020, 02, 02) },
                new Post { Id = 3, Title = " Post 3", ShortDescription = "short 3", PostContent = "Content3", UrlSlug = "Post 3 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 4, Title = " Post 4", ShortDescription = "short 4", PostContent = "Content4", UrlSlug = "Post 4 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 5, Title = " Post 5", ShortDescription = "short 5", PostContent = "Content5", UrlSlug = "Post 5 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 6, Title = " Post 6", ShortDescription = "short 6", PostContent = "Content6", UrlSlug = "Post 6 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 7, Title = " Post 7", ShortDescription = "short 7", PostContent = "Content7", UrlSlug = "Post 7 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 8, Title = " Post 8", ShortDescription = "short 8", PostContent = "Content8", UrlSlug = "Post 8 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 9, Title = " Post 9", ShortDescription = "short 9", PostContent = "Content9", UrlSlug = "Post 9 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) },
                new Post { Id = 10, Title = " Post 10", ShortDescription = "short 10", PostContent = "Content10", UrlSlug = "Post 10 UrlSlug", Published = true, PostedOn = new DateTime(2020, 01, 01), Modified = new DateTime(2020, 02, 01) }

           };

            foreach (var post in posts)
            {
                context.Posts.Add(post);
            }

            context.SaveChanges();


            var tags = new Tag[]
           {
                new Tag{ Id = 1, Name = "Tag 1", UrlSlug = "http: tag1.com", Count = 1 },
                new Tag{ Id = 2, Name = "Tag 2", UrlSlug = "http: tag2.com", Count = 2 },
                new Tag{ Id = 3, Name = "Tag 3", UrlSlug = "http: tag3.com", Count = 3 },
                new Tag{ Id = 4, Name = "Tag 4", UrlSlug = "http: tag4.com", Count = 4 },
                new Tag{ Id = 5, Name = "Tag 5", UrlSlug = "http: tag5.com", Count = 5 },
                new Tag{ Id = 6, Name = "Tag 6", UrlSlug = "http: tag6.com", Count = 6 },
                new Tag{ Id = 7, Name = "Tag 7", UrlSlug = "http: tag7.com", Count = 7 },
                new Tag{ Id = 8, Name = "Tag 8", UrlSlug = "http: tag8.com", Count = 8 },
                new Tag{ Id = 9, Name = "Tag 9", UrlSlug = "http: tag9.com", Count = 9 },
                new Tag{ Id = 10, Name = "Tag 10", UrlSlug = "http: tag10.com", Count = 10 }

           };

            foreach (var tag in tags)
            {
                context.Tags.Add(tag);
            }

            context.SaveChanges();
        }
    }
}
