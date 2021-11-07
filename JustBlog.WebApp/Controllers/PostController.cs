using AutoMapper;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: PostController
        public async Task<ActionResult> Index()
        {
            var posts = await _unitOfWork.Posts.GetAll();
            var model = _mapper.Map<List<Post>, List<ListPostVM>>(posts.ToList());
            return View(model);
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _unitOfWork.Posts.IsExists(p => p.Id == id);
            if (!isExists)
            {
                return NotFound();
            }
            var post = await _unitOfWork.Posts.Find(p => p.Id == id);
            var model = _mapper.Map<PostVM>(post);
            return View(model);
        }

        //GET: PostController/LastestPost
        public async Task<ActionResult> LastestPost()
        {
            var posts = await _unitOfWork.Posts.GetAll();
            var model = _mapper.Map<List<Post>, List<ListPostVM>>(posts.ToList());
            model.OrderByDescending(m => m.PostedOn);
            return View(model);
            
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostVM model)
        {
            try
            {
               /* if(!ModelState.IsValid)
                {
                    return View(model);
                }

                var post = _mapper.Map<Post>(model);
                post.PostedOn = DateTime.Now;
                post.Modified = DateTime.Now;

                await _unitOfWork.Posts.Create(post);
                await _unitOfWork.Save();*/

                return RedirectToAction(nameof(Index));
            }
            catch
            {
               /* ModelState.AddModelError("", "Something went wrong...");*/
                return View();
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
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

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
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