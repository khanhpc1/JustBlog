using App.Utilities;
using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using JustBlog.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Areas.Blog.Controllers
{
    [Authorize(Roles = "Admin,Contributor")]
    [Area("Blog")]
    [Route("blog/post/[action]/{id?}")]
    public class PostController : Controller
    {
        private readonly JustBlogDbContext _context;

        private readonly IPostRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(JustBlogDbContext context, IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Blog/Post
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {
            var posts = await _unitOfWork.GetRepository<Post>().GetAllAsync(
                include: p => p.Include(x => x.Category));
            var postls = _mapper.Map<List<Post>>(posts.ToList())                                        
                                        .OrderByDescending(p => p.PostedOn);

            var postsTotal = postls.Count();
            if (pageSize <= 0) pageSize = 5;
            int countPages = (int)Math.Ceiling((double)postsTotal / pageSize);

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
            ViewBag.postsTotal = postsTotal;

            ViewBag.postIndex = (currentPage - 1) * pageSize;

            var postsInPage = postls.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);

            return View(postsInPage);
        }

        // GET: Blog/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Blog/Post/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();

            ViewData["categories"] = new SelectList(categories.ToList(), "Id", "Name");

            return View();
        }

        // POST: Blog/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostVM model)
        {
            try
            {        
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
         
                var post = _mapper.Map<Post>(model);
                post.PostedOn = DateTime.Now;
                post.Modified = DateTime.Now;
                if (post.UrlSlug == null)
                {
                    post.UrlSlug = AppUtilities.GenerateSlug(post.Title);
                }

                /* ViewData["CategoryId"] = new SelectList(await _unitOfWork.Categories.GetAll(), "Id", "Title", post.CategoryId);*/
                ViewData["CategoryId"] = new SelectList(await _unitOfWork.GetRepository<Category>().GetAllAsync(), "Id", "Id", post.CategoryId);
                
                await _unitOfWork.GetRepository<Post>().CreateAsync(post);
                await _unitOfWork.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: Blog/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.GetRepository<Post>().GetFirstOrDefaultAsync(p =>p.Id==id);
            if (post == null)
            {
                return NotFound();
            }
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();

            ViewData["categories"] = new SelectList(categories.ToList(), "Id", "Name");
            return View(post);
        }

        // POST: Blog/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostVM model)
        {
            
            try
            {
                // TODO: Add update logic here
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                var post = _mapper.Map<Post>(model);
                _unitOfWork.GetRepository<Post>().Update(post);

            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            ViewData["categories"] = new SelectList(categories.ToList(), "Id", "Id", model.CategoryId);

            _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));      
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _unitOfWork.GetRepository<Post>().Delete(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}