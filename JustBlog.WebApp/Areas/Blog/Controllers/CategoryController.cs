﻿using System;
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

namespace JustBlog.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
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
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.GetAll();
            var model = _mapper.Map<List<Category>, List<CategoryVM>>(categories.ToList());
            return View(model);
        }

        // GET: Blog/Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var isExist = await _unitOfWork.Categories.IsExists(c => c.Id == id);
            if (!isExist)
            {
                return NotFound();
            }

            var category = await _unitOfWork.Categories.Find(c => c.Id ==id);
               
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
               

                await _unitOfWork.Categories.Create(category);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isExists = await _unitOfWork.Categories.IsExists(q => q.Id == id);
            if (!isExists)
            {
                return NotFound();
            }
            var category = await _unitOfWork.Categories.Find(q => q.Id == id);
            var model = _mapper.Map<CategoryVM>(category);
            return View(model);
        }

        // POST: Blog/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM model)
        {
           
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<Category>(model);
                _unitOfWork.Categories.Update(leaveType);
                await _unitOfWork.Save();

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
                var category = await _unitOfWork.Categories.Find(filter: q => q.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                _unitOfWork.Categories.Delete(category);
                await _unitOfWork.Save();

            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}