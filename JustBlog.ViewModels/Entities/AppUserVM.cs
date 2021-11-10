using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.ViewModels.Entities
{
    public class AppUserVM
    {
        [MaxLength(100)]
        public string FullName { set; get; }

        [MaxLength(255)]
        public string Address { set; get; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { set; get; }
    }

    public class UserListVM
    {
        public int TotalUsers { get; set; }
        public int CountPages { get; set; }

        public int ITEMS_PER_PAGE { get; set; } = 5;

        public int CurrentPage { get; set; }

        public List<UserAndRoleVM> Users { get; set; }

    }

    public class UserAndRoleVM : AppUser
    {
        public string RoleNames { get; set; }
    }
}
