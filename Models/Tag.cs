using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Tag
    {
        #region Keys
        #endregion

        #region Post Properties
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }
        public virtual Post Post { get; set; }
        #endregion

        #region Navigation
        #endregion
    }
}
