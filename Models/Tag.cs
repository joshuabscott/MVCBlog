using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Tag
    {
        #region Keys
        public int Id { get; set; }
        #endregion

        #region Post Properties

        public int PostId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Navigation
        public virtual Post Posts { get; set; }
        #endregion
    }
}
