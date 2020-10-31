using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using System.IO;
using System.Collections;

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MVCBlog.ViewModels;
using MVCBlog.Models;
using MVCBlog.Enums;
using MVCBlog.Data;

namespace MVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var posts = _context.Posts.Where(p => p.IsPublished).Include(p => p.Blog);
            var blogs = _context.Blogs;
            var tags = _context.Tags;
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync(),
                Tags = await tags.ToListAsync()
            };
            return View(categories);
        }

        public async Task<IActionResult> Results(string SearchString)
        {
            var posts = from p in _context.Posts
                        select p;
            var blogs = _context.Blogs;
            var tags = _context.Tags;
            if (!String.IsNullOrEmpty(SearchString))
            {
                posts = posts.Where(p => p.Title.Contains(SearchString) || p.Abstract.Contains(SearchString) || p.Body.Contains(SearchString));
                //return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            }
            //return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync(),
                Tags = await tags.ToListAsync()
            };
            return View("Index", categories);
        }

        public async Task<IActionResult> Categories()
        {
            var id = RouteData.Values["id"].ToString();
            var posts = _context.Posts.Where(p => p.BlogId == Int32.Parse(id) && p.IsPublished == true).Include(p => p.Blog);
            var blogs = _context.Blogs;
            var tags = _context.Tags;
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync(),
                Tags = await tags.ToListAsync()
            };
            return View("Index", categories);
        }

        public async Task<IActionResult> Tag()
        {
            var name = RouteData.Values["id"].ToString();
            var posts = _context.Tags.Where(t => t.Name == name).Select(t => t.Post);
            var blogs = _context.Blogs;
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync(),
                Tags = await _context.Tags.ToListAsync()
            };
            return View("Index", categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}