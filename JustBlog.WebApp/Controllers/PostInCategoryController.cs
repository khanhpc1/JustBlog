using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Controllers
{
    public class PostInCategoryController : Controller
    {
        private readonly IPostRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostInCategoryController(JustBlogDbContext context, IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: PostInCategoryController
        public ActionResult Index()
        {
           
            return View();
        }

        public async Task<IActionResult> PostList(int id) 
        {
           

           
            return View();
        }

        // GET: PostInCategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostInCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostInCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostInCategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostInCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostInCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostInCategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
