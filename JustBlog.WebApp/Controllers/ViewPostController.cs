using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using JustBlog.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Controllers
{
    
    public class ViewPostController : Controller
    {
        // GET: ViewPostController

        private readonly IPostRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ViewPostController(JustBlogDbContext context, IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {  
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {
            var posts = await _unitOfWork.Posts.GetAll();
            var postls = _mapper.Map<List<Post>, List<ListPostVM>>(posts.ToList())
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

        // GET: ViewPostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var isExists = await _unitOfWork.Posts.IsExists(q => q.Id == id);
            if (!isExists)
            {
                return NotFound();
            }
            var post = await _unitOfWork.Posts.Find(q => q.Id == id);
            var model = _mapper.Map<Post, DetailPostVM >(post);
            return View(model);
        }
          
    }
}
