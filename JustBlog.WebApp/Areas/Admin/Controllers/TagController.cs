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
    [Authorize(Roles = "Admin,Contributor")]
    [Area("Admin")]
    [Route("blog/tag/[action]/{id?}")]
    public class TagController : Controller
    {
        private readonly JustBlogDbContext _context;
        private readonly ITagRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagController(JustBlogDbContext context, ITagRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Blog/Tag
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {
            var categories = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            var model = _mapper.Map<List<Tag>, List<TagVM>>(categories.ToList());

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

            ViewBag.TagIndex = (currentPage - 1) * pageSize;

            var categoriesInPage = model.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);
            return View(categoriesInPage);
        }

        // GET: Blog/Tag/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var isExist = await _unitOfWork.GetRepository<Tag>().IsExistsAsync(c => c.Id == id);
            if (!isExist)
            {
                return NotFound();
            }

            var tag = await _unitOfWork.GetRepository<Tag>().GetFirstOrDefaultAsync(c => c.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TagVM>(tag);
            return View(model);
        }

        // GET: Blog/Tag/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Blog/Tag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagVM model)
        {

            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var tag = _mapper.Map<Tag>(model);

                if (tag.UrlSlug == null)
                {
                    tag.UrlSlug = AppUtilities.GenerateSlug(tag.Name) + "-tag";
                }

                await _unitOfWork.GetRepository<Tag>().CreateAsync(tag);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: Blog/Tag/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var isExists = await _unitOfWork.GetRepository<Tag>().IsExistsAsync(q => q.Id == id);
            if (!isExists)
            {
                return NotFound();
            }
            var tag = await _unitOfWork.GetRepository<Tag>().GetFirstOrDefaultAsync(q => q.Id == id);
            var model = _mapper.Map<TagVM>(tag);
            return View(model);
        }

        // POST: Blog/Tag/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TagVM model)
        {

            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<Tag>(model);
                _unitOfWork.GetRepository<Tag>().Update(leaveType);

                _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }

        }

        // POST: Blog/Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _unitOfWork.GetRepository<Tag>().Delete(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
