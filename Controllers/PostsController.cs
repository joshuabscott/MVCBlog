using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.Utilities;
using MVCBlog.Enums;
using MVCBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MVCBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Posts
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: Posts/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            // for each comment load BlogUser.
            foreach(var comment in post.Comments.ToList())
            {
                comment.BlogUser = await _context.Users.FindAsync(comment.BlogUserId);
            }
            return View(post);
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");
            }
            else
            {
                var blog = _context.Blogs.Find(id);
                if (blog == null)
                {
                    return NotFound();
                }
                var newPost = new Post()
                {
                    BlogId = (int)id
                };
                ViewData["BlogName"] = blog.Name;
                ViewData["BlogId"] = id;
                return View(newPost);
            }
            return View();
        }
        
    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,BlogId,Title,Abstract,Content,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post)
    {
        if (ModelState.IsValid)
            {
                post.Created = DateTimeOffset.Now;
                post.Updated = DateTimeOffset.Now;
                post.Slug = Regex.Replace(post.Title.ToLower(), @"\s", "-");
                //Image to DataVBase
                if(ImageHelper != null)
                {
                    var imageHelper = new ImageHelper();
                    imageHelper.GetImage(post, image);
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            return View(post);
        }


        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
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
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            return View(post);

        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post)
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
                        var imageHelper = new ImageHelper();
                        imageHelper.GetImage(post, image);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
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

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}