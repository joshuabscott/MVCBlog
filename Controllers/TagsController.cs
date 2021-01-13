﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Enums;
using MVCBlog.Models;
using MVCBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MVCBlog.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tags
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tags.Include(t => t.Posts);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tags/Details/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .Include(t => t.Posts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
            return View();
        }

        // POST: Tags/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,PostId,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", tag.PostId);
            return View(tag);
        }

        // GET: Tags/Edit/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", tag.PostId);
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,Name")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", tag.PostId);
            return View(tag);
        }

        // GET: Tags/Delete/5
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .Include(t => t.Posts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}


//        private readonly ApplicationDbContext _context;

//        public TagsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Tags
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Tags.Include(t => t.Post);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Tags/Details/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tag = await _context.Tags
//                .Include(t => t.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (tag == null)
//            {
//                return NotFound();
//            }

//            return View(tag);
//        }

//        // GET: Tags/Create
//        [Authorize(Roles = "Admin")]
//        public IActionResult Create()
//        {
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title");
//            return View();
//        }

//        // POST: Tags/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> Create([Bind("Id,PostId,Name")] Tag tag)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(tag);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", tag.PostId);
//            return View(tag);
//        }

//        // GET: Tags/Edit/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tag = await _context.Tags.FindAsync(id);
//            if (tag == null)
//            {
//                return NotFound();
//            }
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", tag.PostId);
//            return View(tag);
//        }

//        // POST: Tags/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,Name")] Tag tag)
//        {
//            if (id != tag.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(tag);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!TagExists(tag.Id))
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
//            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", tag.PostId);
//            return View(tag);
//        }

//        // GET: Tags/Delete/5
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var tag = await _context.Tags
//                .Include(t => t.Post)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (tag == null)
//            {
//                return NotFound();
//            }

//            return View(tag);
//        }

//        // POST: Tags/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "Admin, Moderator")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var tag = await _context.Tags.FindAsync(id);
//            _context.Tags.Remove(tag);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool TagExists(int id)
//        {
//            return _context.Tags.Any(e => e.Id == id);
//        }
//    }
//}
