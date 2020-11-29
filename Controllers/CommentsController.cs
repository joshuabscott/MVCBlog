using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using MVCBlog.Data;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using MVCBlog.Utilities;
using MVCBlog.Enums;

namespace MVCBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Create()
        {
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "FirstName");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId")] Comment comment, string userComment)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.User.Identity.Name;
                var bloguserid = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                var bloguser = _context.Users.FirstOrDefault(u => u.Email == email);
                var post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);

                comment.Created = DateTime.Now;
                comment.Updated = DateTime.Now;
                comment.Body = userComment;
                comment.BlogUserId = bloguserid;
                comment.BlogUser = bloguser;
                comment.Post = post;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return Redirect($"~/Posts/Details/{comment.PostId}");
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "DisplayName", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,Content,Created,Updated")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (comment.Body.Contains("<!DOCTYPE"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(comment.Body);
                        XmlNode elem = doc.DocumentElement.FirstChild.NextSibling;
                        comment.Body = elem.FirstChild.InnerText.ToString();
                    }
                    comment.Post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                    comment.BlogUser = _context.Users.FirstOrDefault(u => u.Id == comment.BlogUserId);
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
            //return View(comment);
            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}//V2.0 JS 11-28




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using MVCBlog.Data;
//using MVCBlog.Models;
//using MVCBlog.Enums;

//namespace MVCBlog.Controllers
//{
//    public class CommentsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public CommentsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        //Supposed to show comments for a post...
//        public async Task<IActionResult> Comment(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var blog = await _context.Posts.FindAsync(id);
//            if (blog == null)
//            {
//                return NotFound();
//            }

//            var comments = _context.Comments.Where(p => p.PostId == id);       

//            //linking my posts to my blogs topic
//            return View("Index", await comments.ToListAsync());
//        }

//        // GET: Comments
//        [Authorize(Roles = "Administrator, Moderator")]
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Post);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Comments/Details/5
//        [Authorize(Roles = "Administrator, Moderator")]
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments
//                .Include(c => c.BlogUser)
//                .Include(c => c.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        // GET: Comments/Create
//        [Authorize(Roles = "Administrator, Moderator")]
//        public IActionResult Create()
//        {
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id");
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
//            return View();
//        }

//        // POST: Comments/Create
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PostId,Content")] Comment comment, int id)
//        {
//            if (ModelState.IsValid)
//            {
//                //the Id of the Logged-in User can be retrieved program-atically
//                //comment.PostId = id;
//                //comment.Created = DateTime.Now;
//                //var author = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
//                //comment.BlogUserId = author.Id;
//                var email = HttpContext.User.Identity.Name;
//                var authorid = _context.Users.FirstOrDefault(u => u.Email == email).Id;
//                var author = _context.Users.FirstOrDefault(u => u.Email == email);
//                var post = _context.Post.FirstOrDefault(p => p.Id == comment.PostId);

//                comment.Created = DateTime.Now;
//                comment.Updated = DateTime.Now;
//                comment.Body = userComment;
//                comment.BlogUserId = authorid;
//                comment.BlogUser = author;
//                comment.Post = post;


//                _context.Add(comment);
//                await _context.SaveChangesAsync();
//                return RedirectToAction("Details", "Posts", new { id = comment.PostId });       
//            }
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
//            return View(comment);
//        }

//        // GET: Comments/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments.FindAsync(id);
//            if (comment == null)
//            {
//                return NotFound();
//            }
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
//            return View(comment);
//        }

//        // POST: Comments/Edit/5
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,Content,Created,Updated")] Comment comment)
//        {
//            if (id != comment.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(comment);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CommentExists(comment.Id))
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
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
//            return View(comment);
//        }

//        // GET: Comments/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments
//                .Include(c => c.BlogUser)
//                .Include(c => c.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        // POST: Comments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var comment = await _context.Comments.FindAsync(id);
//            _context.Comments.Remove(comment);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CommentExists(int id)
//        {
//            return _context.Comments.Any(e => e.Id == id);
//        }
//    }
//}




//        private readonly ApplicationDbContext _context;
//        //private readonly UserManager<BlogUser> userManager;

//        public CommentsController(ApplicationDbContext context/*, UserManager<BlogUser>manager*/)
//        {
//            //_userManager = manager;
//            _context = context;
//        }

//        // GET: Comments
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Post);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Comments/Details/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments
//                .Include(c => c.BlogUser)
//                .Include(c => c.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        // GET: Comments/Create
//        [Authorize(Roles = "Admin, Moderator")]
//        public IActionResult Create()
//        {
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "FirstName");
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
//            return View();
//        }

//        // POST: Comments/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PostId")] Comment comment, string userComment, string commentBody)
//        {
//            if (ModelState.IsValid)
//            {
//                var email = HttpContext.User.Identity.Name;
//                var blogUserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
//                var blogUser = _context.Users.FirstOrDefault(u => u.Email == email);
//                var post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);

//                comment.Created = DateTimeOffset.Now;
//                comment.Updated = DateTimeOffset.Now;
//                comment.Body = userComment;
//                comment.BlogUserId = blogUserId;
//                comment.BlogUser = blogUser;
//                comment.Post = post;

//                _context.Add(comment);
//                await _context.SaveChangesAsync();
//                //return Redirect($"~/Posts/Details/{comment.PostId}");
//                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
//            }
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
//            return View(comment);
//        }

//        // GET: Comments/Edit/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments.FindAsync(id);
//            if (comment == null)
//            {
//                return NotFound();
//            }
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "DisplayName", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", comment.PostId);
//            return View(comment);
//        }

//        // POST: Comments/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,Body,Created,Updated")] Comment comment)
//        {
//            if (id != comment.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    //_context.Update(comment);
//                    //await _context.SaveChangesAsync();
//                    if (comment.Body.Contains("<!DOCTYPE"))
//                    {
//                        XmlDocument doc = new XmlDocument();
//                        doc.LoadXml(comment.Body);
//                        XmlNode elem = doc.DocumentElement.FirstChild.NextSibling;
//                        comment.Body = elem.FirstChild.InnerText.ToString();
//                    }
//                    comment.Post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
//                    comment.BlogUser = _context.Users.FirstOrDefault(u => u.Id == comment.BlogUserId);
//                    _context.Update(comment);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CommentExists(comment.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                //return RedirectToAction(nameof(Index));
//                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
//            }
//            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
//            //return View(comment);
//            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
//        }

//        // GET: Comments/Delete/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var comment = await _context.Comments
//                .Include(c => c.BlogUser)
//                .Include(c => c.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        // POST: Comments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var comment = await _context.Comments.FindAsync(id);
//            _context.Comments.Remove(comment);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CommentExists(int id)
//        {
//            return _context.Comments.Any(e => e.Id == id);
//        }
//    }
//}