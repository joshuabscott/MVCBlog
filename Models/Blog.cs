using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Blog
    {
        #region Keys
        #endregion

        #region Blog Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        //  public List<Post> Posts { get; set; }
        #endregion
        //  public Blog()
        //  {
        //      Posts = new List<Post>();
        //   }
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        #region Navigation
        #endregion
    }
}
