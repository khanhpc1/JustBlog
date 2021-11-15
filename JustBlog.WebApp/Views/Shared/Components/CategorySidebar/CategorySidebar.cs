using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JustBlog.Web.Views.Shared.Components.CategorySidebar
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public List<Category> Categories { set; get; }
            public string UrlSlugCategory { set; get; }
        }

        public const string COMPONENTNAME = "CategorySidebar";

        public CategorySidebar()
        { }

        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }
    }
}