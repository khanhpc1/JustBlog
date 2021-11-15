using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JustBlog.Core;
using JustBlog.Models.Entities;
using JustBlog.Core.Contracts;
using AutoMapper;
using JustBlog.ViewModels.EntityViewModels;
using JustBlog.Web.Helpers;
using App.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace JustBlog.Web.Areas.Blog.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Contributor")]
    [Route("blog/category/[action]/{id?}")]
    public class CategoryController : Controller
    {
        private readonly JustBlogDbContext _context;
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(JustBlogDbContext context, ICategoryRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Blog/Category
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var model = _mapper.Map<List<Category>, List<CategoryVM>>(categories.ToList());

            var categoriesTotal = model.Count();
            if (pageSize <= 0) pageSize = 5;
            int countPages = (int)Math.Ceiling((double)categoriesTotal / pageSize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                CountPages = countPages,
                CurrentPage = currentPage,
                GenerateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pageSize
                })
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.categoriesTotal = categoriesTotal;

            ViewBag.CategoryIndex = (currentPage - 1) * pageSize;

            var categoriesInPage = model.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);
            return View(categoriesInPage);
        }

        // GET: Blog/Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var isExist = await _unitOfWork.GetRepository<Category>().IsExistsAsync(c => c.Id == id);
            if (!isExist)
            {
                return NotFound();
            }

            var category = await _unitOfWork.GetRepository<Category>().GetFirstOrDefaultAsync(c => c.Id ==id);
               
            if (category == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CategoryVM>(category);
            return View(model);
        }

        // GET: Blog/Category/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Blog/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM model)
        {

            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var category = _mapper.Map<Category>(model);
               

                await _unitOfWork.GetRepository<Category>().CreateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var isExists = await _unitOfWork.GetRepository<Category>().IsExistsAsync(q => q.Id == id);
            if (!isExists)
            {
                return NotFound();
            }
            var category = await _unitOfWork.GetRepository<Category>().GetFirstOrDefaultAsync(q => q.Id == id);
            var model = _mapper.Map<CategoryVM>(category);
            return View(model);
        }

        // POST: Blog/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryVM model)
        {
           
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var category = _mapper.Map<Category>(model);

                if (category.UrlSlug == null)
                {
                    category.UrlSlug = AppUtilities.GenerateSlug(category.Name ) + "-cate";
                }
                _unitOfWork.GetRepository<Category>().Update(category);

                 _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
           
        }

        // POST: Blog/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                 _unitOfWork.GetRepository<Category>().Delete(id);
               await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
