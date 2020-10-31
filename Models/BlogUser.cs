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
        [StringLength(40)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(40)]
        public string LastName { get; set; }

        public string DisplayName { get; set; }
        //public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        //public BlogUser()
        //{
        //    Comments = new HashSet<Comment>();
        //    DisplayName = "New User";
        //}
        public virtual ICollection<Comment> Comments { get; set; }
        // Constructor for BlogUser
        public BlogUser()
        {
            Comments = new HashSet<Comment>();
        }
        #endregion

        #region Navigation
        #endregion

    }
}
