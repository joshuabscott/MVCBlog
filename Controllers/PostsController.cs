using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using MVCBlog.Utilities;
using MVCBlog.Enums;

namespace MVCBlog.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageHelper imageHelper = new ImageHelper();

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
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
            // for each comment load blogUser
            foreach (var comment in post.Comments.ToList())
            {
                comment.BlogUser = await _context.Users.FindAsync(comment.BlogUserId);
            }

            //Image Helper Decoder
            if(post.Image != null)
            {
                ViewData["Image"] = imageHelper.DecodeImage(post.Image, post.FileName);
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Administrator")]
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
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,BlogId,Title,Abstract,Content,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post, IFormFile image)
        {
        //    if (ModelState.IsValid)
        //    {
        //        post.Created = DateTime.Now;
        //        post.Updated = DateTime.Now;
        //        post.Slug = Regex.Replace(post.Title.ToLower(), @"\s", "-");
        //        //Write image to db
        //        if (image != null)
        //        {
        //            var imageHelper = new ImageHelper();
        //            imageHelper.WriteImage(post, image);
        //        }
        //        _context.Add(post);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name", post.BlogId);
        //    return View(post);
        if (ModelState.IsValid)   //Update method to receive & store image
            {
                post.Created = DateTime.Now;
                if (image != null)
                {
                    post.FileName = image.FileName;

                    post.Image = imageHelper.EncodeImage(image);
                }

                    _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        //New BlogPosts Action
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> BlogPosts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["Count"] = blog.Posts.Count();
            ViewData["BlogName"] = blog.Name;
            ViewData["BlogId"] = blog.Id;

            var blogPosts = await _context.Posts.Where(p => p.BlogId == blog.Id).ToListAsync();
            return View(blogPosts);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,Slug,IsPublished,Image,Created,Updated,ImageDataUrl")] Post post, IFormFile image)
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
            return View(post);
        }
        //{
        //    if (id != post.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (image != null)
        //            {
        //                var imageHelper = new ImageHelper();
        //                imageHelper.WriteImage(post, image);
        //            }
        //            post.Updated = DateTime.Now;
        //            _context.Update(post);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PostExists(post.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
        //    return View(post);
        //}

        // GET: Posts/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
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
        [Authorize(Roles = "Administrator, Moderator")]
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