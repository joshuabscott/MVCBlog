using System;
using System.Collections.Generic;
using System.IO;  //MemoryStream
using System.Linq;
using System.Text.RegularExpressions; //Regex.Replace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.Utilities;

namespace MVCBlog.Controllers
{
    //[Authorize(Roles = "Administrator, Moderator")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get : BlogPosts
        public async Task<IActionResult> BlogPosts(int? id)  //id == blog.Id
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);  //id == blog.Id
            if (blog == null)
            {
                return NotFound();
            }

            ViewData["BlogName"] = blog.Name;
            ViewData["BlogId"] = blog.Id;

            var blogPosts = await _context.Posts.Where(p => p.BlogId == id).ToListAsync();  //id == blog.Id
            return View(blogPosts);
        }

        // GET: Posts Index
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blogs);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5         ----------------------------------Add Comments and Images
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blogs)
                .Include(p => p.Comments) // Add Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            // for each comment load author/BlogUser
            foreach (var comment in post.Comments.ToList())
            {
                comment.BlogUser = await _context.Users.FindAsync(comment.BlogUserId);
            }

            // retrieve image and decode for view
            if (post.Image != null && post.Image.Length > 0)
            {
                ViewData["Image"] = ImageHelper.DecodeImage(post);
            }

            return View(post);
        }

        // GET: Posts/Create                ----------------------------Heavily modified
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)// add Line
            {
                ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");// add Line
            }
            else
            {
                var blog = await _context.Blogs.FindAsync(id);// add Line
                if (blog == null)// add Line
                {
                    return NotFound();// add Line
                }

                var newPost = new Post()
                {
                    BlogId = (int)id  // add Line
                };
                ViewData["BlogName"] = blog;   // add Line
                ViewData["BlogId"] = id;           // add Line
                return View(newPost);
            }
            return View();
        }

        // POST: Posts/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,IsPublished")] Post post, IFormFile image) //delete the Id in a create when it is scaffolded like this
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                post.Updated = DateTime.Now;
                post.Slug = Regex.Replace(post.Title.ToLower(), @"\s", "-");
                //Write Image
                if (image != null)
                {
                    post.FileName = image.FileName;
                    post.Image = ImageHelper.EncodeImage(image);
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
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
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Abstract,Content,IsPublished")] Post post, IFormFile image) //add IformFile image
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.Updated = DateTime.Now;

                    if (image != null)//add line
                    {
                        post.FileName = image.FileName;//add line
                        post.Image = ImageHelper.EncodeImage(image);//add line
                    }

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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.Image != null && post.Image.Length > 0)
            {
                ViewData["Image"] = ImageHelper.DecodeImage(post);
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
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}