using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using JustBlog.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Controllers
{
    public class PostController : Controller
    {
        // GET: ViewPostController

        private readonly IPostRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JustBlogDbContext _context;

        public PostController(JustBlogDbContext context, IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {  
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {

            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            ViewData["categories"] = categories;

            var repo = await _unitOfWork.GetRepository<Post>().GetAllAsync();
            
            var posts = _mapper.Map<List<Post>, List<ListPostVM>>(repo.ToList())
                              .OrderByDescending(p => p.PostedOn);

            var postsTotal = posts.Count();
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

            var postsInPage = posts.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);

            return View(postsInPage);
        }

        // GET: ViewPostController/Details/5
        [Route("post/{year}/{month}/{urlSlug}")]
        public async Task<ActionResult> Details(int year, int month, string urlSlug)
        {
            //q.PostedOn.Year == year && q.PostedOn.Month ==month && q.Title ==title
            var isExists = await _unitOfWork.GetRepository<Post>().IsExistsAsync(q =>
                q.PostedOn.Year == year && q.PostedOn.Month == month && q.UrlSlug == urlSlug);
            if (!isExists)
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.FindPost(year, month, urlSlug);
            var model = _mapper.Map<Post, DetailPostVM >(post);
            return View(model);
        }


        [Route("{slug?}", Name = "listpost")]
        public async Task<IActionResult> PostByCategory([FromQuery(Name = "p")] int currentPage, int pageSize, [FromRoute(Name = "slug")] string slugCategory)
        {

            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            ViewData["categories"] = categories;
            ViewData["slugCategory"] = slugCategory;

            // get all posts

            /* var repo = await _unitOfWork.GetRepository<Post>().GetAllAsync(
                  include: p => p.Include(x => x.Category)
                                 .Include(c => c.PostTagMaps)
                                 .ThenInclude(pt => pt.Tag)
                                 );*/
            var repo = _context.Posts
              .Include(p => p.Category) 
              .Include(p => p.PostTagMaps) 
              .ThenInclude(pt => pt.Tag)
              .AsQueryable();

            Category category = null;
            if (!string.IsNullOrEmpty(slugCategory))
            {
                 
                 category = await _unitOfWork.GetRepository<Category>().GetFirstOrDefaultAsync(c =>c.UrlSlug==slugCategory);
                
                if (category == null && slugCategory != "Post")
                {
                    return NotFound("Not Found Category");
                }

            }

            if (category != null)
            {
                // Filter Post Have in  category
                repo = repo.Where(p => p.CategoryId == category.Id);

            }

            var posts = _mapper.Map<List<Post>, List<ListPostVM>>(repo.ToList())
                              .OrderByDescending(p => p.PostedOn);

            var postsTotal = posts.Count();
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

            var postsInPage = posts.Skip((currentPage - 1) * pageSize)
                             .Take(pageSize);

            return View(postsInPage);
        }

        public ActionResult AboutCard()
        {
            return PartialView("_PartialAboutCard");
        }

        public async Task<IActionResult> LastestPosts()
        {

            var repo = await _unitOfWork.GetRepository<Post>().GetAllAsync();

            var posts = _mapper.Map<List<Post>, List<LastestPostVM>>(repo.ToList())
                              .OrderByDescending(p => p.PostedOn);

            //var lastestPost = posts.TakeLast(5);

            return View(posts);
        } 

    }
}
