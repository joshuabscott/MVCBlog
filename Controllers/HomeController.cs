using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync()
            };
            return View(categories);
        }

        public async Task<IActionResult> Results(string SearchString)
        {
            var posts = from p in _context.Posts
                        select p;
            var blogs = _context.Blogs;
            if (!String.IsNullOrEmpty(SearchString))
            {
                posts = posts.Where(p => p.Title.Contains(SearchString) || p.Abstract.Contains(SearchString) || p.Body.Contains(SearchString));
                //return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            }
            //return View("Index", await posts.Include(p => p.Blog).ToListAsync());
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync()
            };
            return View("Index", categories);
        }

        public async Task<IActionResult> Categories()
        {
            var id = RouteData.Values["id"].ToString();
            var posts = _context.Posts.Where(p => p.BlogId == Int32.Parse(id) && p.IsPublished == true).Include(p => p.Blog);
            var blogs = _context.Blogs;
            CategoryVMJS categories = new CategoryVMJS()
            {
                Blogs = await blogs.ToListAsync(),
                Posts = await posts.ToListAsync()
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