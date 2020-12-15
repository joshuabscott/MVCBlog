using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Models;

namespace MVCBlog.ViewModels
{
    public class BlogPostsVM
    {
        public Blog Blog { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Tag> Tags { get; set; }

        public int PageNumber { get; set; }
        public int TotalPosts { get; set; }
    }
}
