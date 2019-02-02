using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogAppVer5.BdModels;

namespace BlogAppVer5.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly BlogContext db;

        public AdminHomeController(BlogContext context)
        {
            db = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await db.Posts.Include(pt=>pt.PostTags).ThenInclude(t=>t.Tag).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreateTime,Body,Published")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Add(post);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreateTime,Body,Published")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(post);
                    await db.SaveChangesAsync();
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

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts
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
            var post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Posts/CreateTag
        public IActionResult CreateTags()
        {
          return View();
        }

        // POST: Posts/CreateTag
      
        [HttpPost]
        public async Task<IActionResult> CreateTags([Bind("Id,TextTag")] Tag tag)
        {
            db.Add(tag);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        // GET
        public async Task<IActionResult> ShowTags()
        {
            return View(await db.Tags.Include(pt => pt.PostTags).ThenInclude(p=>p.Post).ToListAsync());
        }

        //GET
        public async Task<IActionResult> LinkTag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var tag = await db.Tags.Include(pt=>pt.PostTags).ThenInclude(p=>p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }


        private bool PostExists(int id)
        {
            return db.Posts.Any(e => e.Id == id);
        }
    }
}
