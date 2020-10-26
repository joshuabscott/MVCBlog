using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Models;

namespace MVCBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MVCBlog.Models.Blog> Blogs { get; set; }
        public DbSet<MVCBlog.Models.Comment> Comments { get; set; }
        public DbSet<MVCBlog.Models.Post> Posts { get; set; }
        public DbSet<MVCBlog.Models.Tag> Tags { get; set; }
    }
}
