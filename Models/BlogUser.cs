using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class BlogUser : IdentityUser
    {
        #region BlogUser
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string DisplayName { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public BlogUser()
        {
            Comments = new HashSet<Comment>();
            DisplayName = "New User";
        }
        #endregion

        #region Navigation
        #endregion

    }
}
