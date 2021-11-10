using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.ViewModels.User
{
    public class UserVM
    {
    }

    public class AddUserRoleVM
    {
        public AppUser User { get; set; }

        [DisplayName("Roles assigned to users")]
        public string[] RoleNames { get; set; }

        public List<IdentityRoleClaim<string>> ClaimsInRole { get; set; }
        public List<IdentityUserClaim<string>> ClaimsInUserClaim { get; set; }

    }
}
