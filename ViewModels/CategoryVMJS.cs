﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Models;

namespace MVCBlog.ViewModels
{
    public class CategoryVMJS
    {
        public ICollection<Blog> Blogs { get; set; }
        // The entire Post model and all of its information
        public ICollection<Post> Posts { get; set; }
        public ICollection<Tag> Tags { get; set; }

        //        public <Post> FeaturedPost { get; set; }
        //        // Load most last post property
        //        public <Post> LatestPost { get; set; }
        //}
        public int PageNumber { get; set; }
        public int TotalPosts { get; set; }
    }
}
