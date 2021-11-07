using Bogus;
using JustBlog.Models.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace JustBlog.Core.Migrations
{
    public partial class Seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Insert data
            //fake data: Bogus
            Randomizer.Seed = new Random(8675309);

            var categoryFaker = new Faker<Category>()
                .RuleFor(p => p.Id, f => f.IndexVariable++)
                .RuleFor(p => p.Name, f => f.Lorem.Sentence(5, 5))
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph(20))
                .RuleFor(p => p.UrlSlug, f => f.Internet.Url());


            var postFaker = new Faker<Post>()
                .RuleFor(p => p.Id, f => f.IndexVariable++)
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(5, 5))
                .RuleFor(p => p.ShortDescription, f => f.Lorem.Paragraph(20))
                .RuleFor(p => p.PostContent, f => f.Lorem.Paragraphs(1, 4))
                .RuleFor(p => p.UrlSlug, f => f.Internet.Url())
                .RuleFor(p => p.PostedOn, f => f.Date.Between(new DateTime(2021, 1, 1), new DateTime(2021, 11, 1)))
                .RuleFor(p => p.Published, f => f.Random.Bool())
                .RuleFor(p => p.CategoryId, f => f.Random.Int(1, 5))
                ;

            var tagFaker = new Faker<Tag>()
               .RuleFor(p => p.Id, f => f.IndexVariable++)
               .RuleFor(p => p.Name, f => f.Lorem.Sentence(5, 5))
               .RuleFor(p => p.Description, f => f.Lorem.Paragraph(20))
               .RuleFor(p => p.UrlSlug, f => f.Internet.Url())
               .RuleFor(p => p.Count, f => f.Random.Int());


            for (int i = 0; i < 21; i++)
            {
                Category category = categoryFaker.Generate();
                migrationBuilder.InsertData(
                    table: "Categories",
                  columns: new[] { "Id", "Name", "UrlSlug", "Description" },
                  values: new object[]
                  {
                      category.Id,
                      category.Name,
                      category.UrlSlug,
                      category.Description,

                  });

                Post post = postFaker.Generate();
                migrationBuilder.InsertData(
                  table: "Posts",
                  columns: new[] { "Id", "Title", "UrlSlug", "ShortDescription", "PostContent", "PostedOn", "Published", "CategoryId" },
                  values: new object[]
                  {
                      post.Id,
                      post.Title,
                      post.UrlSlug,
                      post.ShortDescription,
                      post.PostContent,
                      post.PostedOn,
                      post.Published,
                      post.Category
                  });

                Tag tag = tagFaker.Generate();
                migrationBuilder.InsertData(
                    table: "Tags",
                    columns: new[] { "Id", "Name", "Description", "UrlSlug", "Count" },
                    values: new object[]
                    {
                      tag.Id,
                      tag.Name,
                      tag.UrlSlug,
                      tag.Description,
                      tag.Count
                    }
                    );
            }



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
