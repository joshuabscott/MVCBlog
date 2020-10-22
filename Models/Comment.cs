﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Comment
    {
        #region Keys
        public int Id { get; set; }
        public int PostId { get; set; }
        public string BlogUserId { get; set; }
        #endregion

        #region Comment Properties
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual Post Post { get; set; }
        public virtual BlogUser BlogUser { get; set; }

        #endregion

        #region Navigation
        #endregion
    }
}
