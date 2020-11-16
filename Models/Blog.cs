using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Blog
    {
        //This is intended for categorization of posts
        #region Keys

        public int Id { get; set; }

        #endregion

        #region Blogs Properties
        public string Name { get; set; }
        public string URL { get; set; }

        #endregion
        //public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<Post> Posts { get; set; }

        public Blog()
        {
            Posts = new HashSet<Post>();
        }

        #region Navigation

        #endregion
    }
}
