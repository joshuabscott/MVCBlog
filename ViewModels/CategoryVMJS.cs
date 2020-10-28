using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.ViewModels
{
    public class CategoryVMJS
    {
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
