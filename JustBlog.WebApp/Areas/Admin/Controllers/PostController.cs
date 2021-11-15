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
    [Area("Admin")]
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

        [TempData]
        public string StatusMessage { get; set; } 

        // GET: Blog/Post
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {
            var posts = await _unitOfWork.GetRepository<Post>().GetAllAsync(
                include: p => p.Include(x => x.Category)
                               .Include(c => c.PostTagMaps)
                               .ThenInclude(pt => pt.Tag));
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

            // Tags assign post, create MultiSelectList
            var tags = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            ViewData["tags"] = new MultiSelectList(tags.ToList(), "Id", "Name");
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
           
                ViewData["CategoryId"] = new SelectList(await _unitOfWork.GetRepository<Category>()
                                                            .GetAllAsync(), "Id", "Id", post.CategoryId);
                await _unitOfWork.GetRepository<Post>().CreateAsync(post);
               
                if (model.TagIds != null)
                {
                    foreach (var tagId in model.TagIds)
                    {
                        _context.Add(new PostTagMap()
                        {
                            TagId = tagId,
                            Post = post
                        });
                    }
                }
          
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

            // var post = await _unitOfWork.GetRepository<Post>().GetFirstOrDefaultAsync(p => p.Id ==id);

            var post = await _context.Posts.Include(p => p.PostTagMaps).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var postEdit = new PostVM()
            {
                Id = post.Id,
                Title = post.Title,
                PostContent = post.PostContent,
                ShortDescription = post.ShortDescription,
                UrlSlug = post.UrlSlug,
                Published = post.Published,
                CategoryId = post.CategoryId,
                TagIds = post.PostTagMaps.Select(pt =>pt.TagId).ToArray()       
            };

          var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            ViewData["categories"] = new SelectList(categories.ToList(), "Id", "Name");

            var tags = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            ViewData["tags"] = new MultiSelectList(tags.ToList(), "Id", "Name");
            return View(postEdit);
        }

        // POST: Blog/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostVM model)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            ViewData["categories"] = new SelectList(categories.ToList(), "Id", "Id", model.CategoryId);

            var tags = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            ViewData["tags"] = new MultiSelectList(tags.ToList(), "Id", "Name", model.PostTagMaps);
            try
            {
                // TODO: Add update logic here
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                var postUpdate =  await _unitOfWork.GetRepository<Post>().GetFirstOrDefaultAsync(
                include: p => p.Include(x => x.Category)
                               .Include(c => c.PostTagMaps));

                if (postUpdate == null)
                {
                    return NotFound();
                }

                postUpdate.Title = model.Title;
                postUpdate.ShortDescription = model.ShortDescription;
                postUpdate.PostContent = model.PostContent;
                postUpdate.Published = model.Published;
                postUpdate.UrlSlug = model.UrlSlug;
                postUpdate.Modified = DateTime.Now;

                if (model.TagIds == null) model.TagIds = new int[] { };

                var oldTagIds = postUpdate.PostTagMaps.Select(t => t.TagId).ToArray();
                var newTagIds = model.TagIds;

                var removeTagPosts = from postTag in postUpdate.PostTagMaps
                                      where (!newTagIds.Contains(postTag.TagId))
                                      select postTag;
                _context.PostTagMaps.RemoveRange(removeTagPosts);

                var post = _mapper.Map<Post>(model);

                var addTagIds = from tagId in newTagIds
                                 where !oldTagIds.Contains(tagId)
                                 select tagId;

                foreach (var tagId in addTagIds)
                {
                    _context.PostTagMaps.Add(new PostTagMap()
                    {
                        PostId = id,
                        TagId = tagId
                    });
                }

               
                _unitOfWork.GetRepository<Post>().Update(post);

                _unitOfWork.SaveChanges();

                StatusMessage = "Upadate Success";
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