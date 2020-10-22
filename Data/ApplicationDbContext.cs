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
        public DbSet<MVCBlog.Models.Blog> Blog { get; set; }
        public DbSet<MVCBlog.Models.Comment> Comment { get; set; }
        public DbSet<MVCBlog.Models.Post> Post { get; set; }
        public DbSet<MVCBlog.Models.Tag> Tag { get; set; }
    }
}
