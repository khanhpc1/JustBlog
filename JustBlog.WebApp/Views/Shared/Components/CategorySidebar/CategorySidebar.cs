using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Views.Shared.Components.CategorySidebar
{
    public class CategorySidebar
    {
        private readonly JustBlogDbContext _context;
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategorySidebar(JustBlogDbContext context, ICategoryRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Blog/Category
       /* public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAll();
            var model = _mapper.Map<List<Category>, List<SidebarCategoryVM>>(categories.ToList());
            return View(model);
        }*/

    }
}
