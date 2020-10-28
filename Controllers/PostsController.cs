using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.Utilities;

namespace MVCBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowPosts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            var posts = _context.Posts.Where(p => p.BlogId == id);       //Linking posts to blog topic
            if (posts == null)
            {
                return NotFound();
            }
            return View("Index", await posts.ToListAsync());
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
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
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 



        // GET: Posts/Create
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
        //    var newPost = new Post()
        //    BlogId = (int)id };

        //return View(newPost);



    //{ 
    //    if (id = null)
    //    {
    //        return NotFound();
    //    }
    //    var model = new Post() { BlogId = (int)id };
    //    return View(model);
    //}
    //{
    //    ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id");
    //     var newpost = new Post {BlogId = (int)id};
    //    return View(newpost);
    //}

    ////var ImageHelper = new ImageHelper();
    //if (post.Image != null)
    //    {
    //    ViewDatga["Image"] = ImageHelper.GetImage(post);
    //    }
    //return View(post);

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogId,Title,Abstract,Body,IsPublished")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTimeOffset.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
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
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Body,Slug,Content,Image,Created,Updated,IsPublished")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                //return RedirectToAction(nameof(Index));
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
