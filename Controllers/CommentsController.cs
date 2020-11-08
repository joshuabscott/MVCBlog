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
using System.Xml;

namespace MVCBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<BlogUser> userManager;

        public CommentsController(ApplicationDbContext context/*, UserManager<BlogUser>manager*/)
        {
            //_userManager = manager;
            _context = context;
        }

        // GET: Comments
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Admin, Moderator")]
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
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Create()
        {
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "FirstName");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId")] Comment comment, string userComment, string commentBody)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.User.Identity.Name;
                var blogUserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                var blogUser = _context.Users.FirstOrDefault(u => u.Email == email);
                var post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);

                comment.Created = DateTimeOffset.Now;
                comment.Updated = DateTimeOffset.Now;
                comment.Body = userComment;
                comment.BlogUserId = blogUserId;
                comment.BlogUser = blogUser;
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
        [Authorize(Roles = "Admin, Moderator")]
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,Body,Created,Updated")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(comment);
                    //await _context.SaveChangesAsync();
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
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
            //return View(comment);
            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
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
        [Authorize(Roles = "Admin, Moderator")]
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
}