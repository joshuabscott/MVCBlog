using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Comment
    {
        #region Keys
        //Primary Key
        public int Id { get; set; }
        //Foreign Key
        public int PostId { get; set; }
        public string BlogUserId { get; set; }
        #endregion

        #region Comment Properties
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        #endregion

        #region Navigation
        public virtual Post Post { get; set; }
        //Type BlogUser of Author called BlogUser
        public virtual BlogUser BlogUser { get; set; }
        #endregion
    }
}