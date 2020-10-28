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
        public string Name { get; set; }
        public string URL { get; set; }

        #endregion

        #region Blog Properties

        //  public List<Post> Posts { get; set; }

        //  public Blog()
        //  {
        //      Posts = new List<Post>();
        //   }

        //public Blog()
        //{
        //    Posts = new HashSet<Post>();
        //}
        #endregion
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        #region Navigation
       
        #endregion
    }
}
