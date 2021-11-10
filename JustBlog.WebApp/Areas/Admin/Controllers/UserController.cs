using App.ExtendMethods;
using JustBlog.Core;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Entities;
using JustBlog.ViewModels.User;
using JustBlog.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("/Admin/User/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JustBlogDbContext _context;

        public UserController(RoleManager<IdentityRole> roleManager, JustBlogDbContext context,
            UserManager<AppUser> userManager )
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }
        
        [TempData]
        public string StatusMessage { get; set; }


        // GET: UserController
        public async Task<ActionResult> Index(UserListVM model, [FromQuery(Name = "p")] int currentPage, int pageSize)
        {
           
            model.CurrentPage = currentPage;

            var users = _userManager.Users.OrderBy(u => u.UserName);
          
             model.TotalUsers = await users.CountAsync();
             model.CountPages = (int)Math.Ceiling((double)model.TotalUsers / model.ITEMS_PER_PAGE);

            if (pageSize <= 0) pageSize = 5;

            if (model.CurrentPage < 1)
                 model.CurrentPage = 1;
             if (model.CurrentPage > model.CountPages)
               model.CurrentPage = model.CountPages;

            var pagingModel = new PagingModel()
             {
                CountPages = model.CountPages,
                CurrentPage = model.CurrentPage,
                GenerateUrl = (pageNumber) => Url.Action("Index", new
                {
                     p = pageNumber,
                    pagesize = pageSize
                 })
                };

             ViewBag.pagingModel = pagingModel;
             ViewBag.totalUsers = model.TotalUsers;
             ViewBag.userIndex = (currentPage - 1) * pageSize;

             var usersPage = users.Skip((model.CurrentPage - 1) * model.ITEMS_PER_PAGE)
                        .Take(model.ITEMS_PER_PAGE)
                         .Select(u => new UserAndRoleVM()
                       {
                           Id = u.Id,
                             UserName = u.UserName,
                         });

             model.Users = await usersPage.ToListAsync();

             foreach (var user in model.Users)
             {
                 var roles = await _userManager.GetRolesAsync(user);
                 user.RoleNames = string.Join(",", roles);
             }
            return View(model);      
        }

        //GET  
        [HttpGet("{id}")]
        public async Task<IActionResult> AddRoleAsync(AddUserRoleVM model, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($" Not found user");
            }

            model.User = await _userManager.FindByIdAsync(id);
            if(model.User == null)
            {
                return NotFound($"Not found user, id = {id}");
            }

            model.RoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray<string>();

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            
            return View(model);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(string id, [Bind("RoleNames")] AddUserRoleVM model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            model.User = await _userManager.FindByIdAsync(id);

            if (model.User == null)
            {
                return NotFound($"Not Found user, id = {id}.");
            }
            

            var OldRoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray();
            var deleteRoles = OldRoleNames.Where(r => !model.RoleNames.Contains(r));
            var addRoles = model.RoleNames.Where(r => !OldRoleNames.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            ViewBag.allRoles = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(model.User, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                ModelState.AddModelError(resultDelete);
                return View(model);
            }

            var resultAdd = await _userManager.AddToRolesAsync(model.User, addRoles);
            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError(resultAdd);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetPasswordAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"User not exist");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Not found user, id = {id}.");
            }

            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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

        private async Task GetClaims(AddUserRoleVM model)
        {
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == model.User.Id
                            select r;
           


            var _claimsInRole = from c in _context.RoleClaims
                                join r in listRoles on c.RoleId equals r.Id
                                select c;
            model.ClaimsInRole = await _claimsInRole.ToListAsync();


            model.ClaimsInUserClaim = await (from c in _context.UserClaims
                                             where c.UserId == model.User.Id
                                             select c).ToListAsync();

        }
    }
}
