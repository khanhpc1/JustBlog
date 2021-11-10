using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.ViewModels.Role
{
    public class ListRoleVM : IdentityRole
    {
        public string[]  Claims { get; set; }
    }

    public class CreateRoleVM
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} mus be between {2} and  {1} chacracters long")]
        public string Name { get; set; }
    }

    public class EditRoleVM : IdentityRole
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} mus be between {2} and  {1} chacracters long")]
        public string Name { get; set; }
        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public IdentityRole Role { get; set; }
    }

    public class EditClaimVM
    {
        [Display(Name = "Type of claim")]
        [Required(ErrorMessage = "Enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} mus be between {2} and {1} chacracters long")]
        public string ClaimType { get; set; }

        [Display(Name = "Value")]
        [Required(ErrorMessage = "Enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} mus be between {2} and  {1} chacracters long")]
        public string ClaimValue { get; set; }

        public IdentityRole Role { get; set; }
    }




}
