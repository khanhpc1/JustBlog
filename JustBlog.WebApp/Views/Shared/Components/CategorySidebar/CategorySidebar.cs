using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ViewComponent]
public class CategorySidebar : ViewComponent
{
    public class CategorySidebarData
    {
        public List<Category> categories { set; get; }
   
    } 

    public const string COMPONENTNAME = "CategorySidebar";
    public CategorySidebar() { }
    public IViewComponentResult Invoke(CategorySidebarData data)
    {
        return View(data);
    }
}