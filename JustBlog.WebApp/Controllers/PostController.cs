﻿using AutoMapper;
using JustBlog.Core;
using JustBlog.Core.Contracts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.EntityViewModels;
using JustBlog.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
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

        public PostController(JustBlogDbContext context, IPostRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {  
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

      

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pageSize)
        {

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