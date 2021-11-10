using App.ExtendMethods;
using JustBlog.Core;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    [Route("Admin/Role/[action]")]
    public class RoleController : Controller
    {

        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly JustBlogDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, JustBlogDbContext context,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }


        // GET: Role
        public async Task<ActionResult> Index()
        {
            var rolels = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var roles = new List<ListRoleVM>();
            foreach (var role in rolels)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var claimsString = claims.Select(c => c.Type + "=" + c.Value);

                var model = new ListRoleVM()
                {
                    Name = role.Name,
                    Id = role.Id,
                    Claims = claimsString.ToArray()
                };
                roles.Add(model);
            }

            return View(roles);
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRoleVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var newRole = new IdentityRole(model.Name);
                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    StatusMessage = $"You just create new role : model.Name";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong...");

                }
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View();
            }
        }

        // GET: Role/Edit/5
        [HttpGet("{roleid}")]
        public async Task<ActionResult> EditAsync(string roleid, [Bind("Name")] EditRoleVM model)
        {
            if (roleid == null) return NotFound("Not Found role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Not Found role");
            }
            model.Name = role.Name;
            model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.Role = role;
            ModelState.Clear();
            return View(model);
        }

        // POST: Role/Edit/5
        [HttpPost("{roleid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConFirm(string roleid, [Bind("Name")] EditRoleVM model)
        {
            try
            {
                if (roleid == null) return NotFound("Not found role");
                var role = await _roleManager.FindByIdAsync(roleid);
                if (role == null)
                {
                    return NotFound("Not Found role");
                }
                model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
                model.Role = role;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(result);
                    return View(model);
                }
                    StatusMessage = $"You just change Name: {model.Name}";
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }
        }

        // GET: Role/Delete/5
        /*[HttpGet("{roleid}")]
        public async Task<ActionResult> DeleteAsync(string roleid)
        {
            if (roleid == null) return NotFound("Not found role.");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Not Found role");
            }
            return View(role);
        }*/

        // POST: Role/Delete/5
        [HttpPost("{roleid}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string roleid)
        {
            try
            {
                if (roleid == null) return NotFound("Not Found role!");
                var role = await _roleManager.FindByIdAsync(roleid);
                if (role == null) return NotFound("Not Found role");

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    StatusMessage = $"You just delete: {role.Name}";
                    
                }
                else
                {
                    ModelState.AddModelError(result);
                   
                }
                
            }
            catch
            {
                ModelState.AddModelError("", "something wrong");
               
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
