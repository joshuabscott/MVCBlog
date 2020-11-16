using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.Utilities;

//using System.Runtime.InteropServices;
//using System.Text.RegularExpressions;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using MVCBlog.ViewModels;
//using MVCBlog.Models;
//using MVCBlog.Enums;
//using MVCBlog.Data;
//using MVCBlog.Utilities;

namespace MVCBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Shows individual posts 
        public async Task<IActionResult> BlogPosts(int? id)
        {
            //I know that I will have to push an instance of Post into the View

            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            ViewData["BlogName"] = blog.Name;
            ViewData["BlogId"] = blog.Id;

            var posts = _context.Posts.Where(p => p.BlogId == id);       
            //linking my posts to topic
            return View(await posts.ToListAsync());

        }
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)       
            //Method; includes parameters int? and id
        {
            var post = await _context.Posts         
                //Link Statement
                .Include(p => p.Blog)
                .Include(p => p.Comments).ThenInclude(b => b.BlogUser)           
                //Eager Loading; from database
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post.Image != null)
            {
                ViewData["Image"] = ImageHelper.GetImage(post);
            }

            if (id == null)
            {                               
                //"Catch"; checks for errors in code and returns NotFound()
                return NotFound();
            }

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize/*(Roles = "Administrator")*/]
        //This is what keeps regular users out. Nobody BUT administrator can access this.
        public IActionResult Create(int? id)
        {
            //See if I have been given Id 
            if (id == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.Find(id);

            if (blog == null)
            {
                return NotFound();
            }

            ViewData["BlogName"] = blog.Name;
            var model = new Post() { BlogId = (int)id };

            //ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id");
            return View(model);
        }
        // POST: Posts/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,Image")] Post post, IFormFile image)
        {

            if (ModelState.IsValid)     //This is what's posting the image
            {

                if (image != null)
                {
                    post.FileName = image.FileName;

                    post.Image = ImageHelper.EncodeImage(image);
                }


                post.Created = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BlogPosts), new { id = post.BlogId });
            }
            //ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize/*(Roles = "Administrator, Moderator")*/]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.Image != null)
            {
                ViewData["Image"] = ImageHelper.GetImage(post);
            }

            //ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,Image,Created,FileName")] Post post, IFormFile image)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {

                    if (image != null)
                    {
                        post.FileName = image.FileName;

                        post.Image = ImageHelper.EncodeImage(image);
                    }

                    post.Updated = DateTime.Now;
                    _context.Update(post);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(BlogPosts), new { id = post.BlogId });
            }
            //ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize/*(Roles = "Administrator, Moderator")*/]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }


            if (post.Image != null)
            {
                ViewData["Image"] = ImageHelper.GetImage(post);
            }


            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BlogPosts), new { id = post.BlogId });     //Tried inserting BlogPosts, but 404 occured
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}

//        private readonly ApplicationDbContext _context;
//        public PostsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Posts
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Index()
//        {
//            //var posts = _context.Posts.Include(p => p.Blog);
//            //return View(await posts.ToListAsync());
//            var applicationDbContext = _context.Posts.Include(p => p.Blog);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Posts/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            var post = await _context.Posts
//                .Include(p => p.Blog)
//                .Include(p => p.Comments)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }
//            foreach(var comment in post.Comments.ToList())
//            {
//                comment.BlogUser = await _context.Users.FindAsync(comment.BlogUserId);
//            }
//            //ViewData["Image"] = ImageHelper.DecodeImage(post.Image, post.FileName);
//            return View(post);
//        }
//        // GET: Posts/Create
//        [Authorize(Roles = "Admin")]
//        public IActionResult Create(int? id)
//        {
//            if (id == null)
//            {
//                ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
//            }
//            else
//            {
//                var blog = _context.Blogs.Find(id);
//                if (blog == null)
//                {
//                    return NotFound();
//                }
//                var newPost = new Post()
//                {
//                    BlogId = (int)id
//                };
//                ViewData["BlogName"] = blog.Name;
//                ViewData["BlogId"] = id;
//                return View(newPost);
//            }
//            return View();
//        }

//    // POST: Posts/Create
//    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> Create([Bind("Id,BlogId,Title,Abstract,Content,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post, IFormFile image)
//        {
//            if (ModelState.IsValid)
//            {
//                post.Created = DateTimeOffset.Now;
//                post.Updated = DateTimeOffset.Now;
//                post.Slug = Regex.Replace(post.Title.ToLower(), @"\s", "-");
//                if (image != null)
//                {
//                    var imageHelper = new ImageHelper();
//                    imageHelper.GetImage(post, image);
//                }
//                _context.Add(post);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));

//            }
//            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
//            return View(post);
//        }
//        // GET: Posts/Edit/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> BlogPosts(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var blog = await _context.Blogs.FindAsync(id);
//            if (blog == null)
//            {
//                return NotFound();
//            }

//            ViewData["BlogName"] = blog.Name;
//            ViewData["BlogId"] = blog.Id;

//            var blogPosts = await _context.Posts.Where(p => p.BlogId == blog.Id).ToListAsync();
//            return View(blogPosts);
//        }

//        // GET: Posts/Edit/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Posts.FindAsync(id);
//            if (post == null)
//            {
//                return NotFound();
//            }
//            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
//            return View(post);
//        }

//        // POST: Posts/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Body,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post, IFormFile image)
//        {
//            if (id != post.Id)
//            {
//                return NotFound();
//            }
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    if (image != null)
//                    {
//                        var imageHelper = new ImageHelper();
//                        imageHelper.GetImage(post, image);
//                    }
//                    post.Updated = DateTimeOffset.Now;
//                    _context.Update(post);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PostExists(post.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
//            return View(post);
//        }
//        // GET: Posts/Delete/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Posts
//                .Include(p => p.Blog)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            return View(post);
//        }

//        // POST: Posts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var post = await _context.Posts.FindAsync(id);
//            _context.Posts.Remove(post);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PostExists(int id)
//        {
//            return _context.Posts.Any(e => e.Id == id);
//        }
//    }
//}