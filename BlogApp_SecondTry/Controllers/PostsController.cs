using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp_SecondTry.BdModels;

namespace BlogApp_SecondTry.Controllers
{
    public class PostsController : Controller
    {
        // Index
        // Create
        // Details
        // Edit
        // Delete
        // EditTag(for Post)
        // AddTag(for Post)
        // DeleteTagfor Post)

        private readonly BlogContext db;

        public PostsController(BlogContext context)
        {
            db = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await db.Posts.ToListAsync());
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
            ViewBag.Date = DateTime.Now;   //!!! cделать так, чтобы время бралось с внешнего сервака! 
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

        // GET: Posts/EditTag/5
        [HttpGet]
        public async Task<IActionResult> EditTag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
         
            var post = await db.Posts.Include(pt => pt.PostTags).ThenInclude(t => t.Tag).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/AddTag/5
        [HttpGet]
        public async Task<IActionResult> AddTag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts
                .Include(pt => pt.PostTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {

                return NotFound();
            }

            List<Tag> tags = await db.Tags.ToListAsync();
            ViewBag.Tags = new SelectList(tags, "Id", "TextTag");
            ViewBag.Post = post;

            return View();
        }
        // Post: Posts/AddTag/5
        [HttpPost]
        public async Task<IActionResult> AddTag(PostTag postTag)  //!!!! почему тут приходит значения только для Id, для самих Post и Tag - значение -Null. Но все выходит ок
        {         
            var post = await db.Posts
                .Include(pt => pt.PostTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(p => p.Id == postTag.PostId); 

            if (TagExists(post,postTag.TagId))
                 return RedirectToAction(nameof(Index));

            post.PostTags.Add(postTag); // эта команда добавляет в таблицу PostTag, и в таблицу tag. несмотря на то, что в полученном postTage нет значения Post И Tag 
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // GET: Posts/DeleteTag/
        public async Task<IActionResult> DeleteTag(int? tagId, int? postId)
        {
            if (tagId == null)
            {
                return NotFound();
            }

            Tag tag = await db.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
            Post post = await db.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (tag==null)
                return NotFound();

            ViewBag.post = post;
            ViewBag.tag = tag;
            return View();
        }
        // POST: Posts/DeleteTag/
        [HttpPost, ActionName("DeleteTag")]
        public async Task<IActionResult> DeleteTagConfirmed(PostTag postTag) //!!!! почему тут приходит значения только для Id, для самих Post и Tag - значение -Null. Но тут не выходжит ок. 
            // Он не удаляет так просто postTag. Нужно найти имеющийся в базе.
        {
            var post = await db.Posts
                .Include(pt => pt.PostTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(p => p.Id == postTag.PostId);

            if (!TagExists(post, postTag.TagId))
                return RedirectToAction(nameof(Index));

            PostTag deletedPostTag = post.PostTags.FirstOrDefault(pt => pt.TagId == postTag.TagId);

            post.PostTags.Remove(deletedPostTag);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return db.Posts.Any(e => e.Id == id);
        }

        private bool TagExists(Post post, int id)
        {
            //!!! так как не указывали наличие базы PostTag в BlogContext(кстати, почему? так везде говорят делать), то я и не могу обратиться к ней с вопросом о наличии. Приходится делать так:
            bool Exist = false;
            foreach (PostTag postTag in post.PostTags)
            {
                Exist = (postTag.TagId ==id);
                if (Exist) return Exist;
            }
            return Exist;            
            //!!!!  как то это можно через sql и Where?
        }
    }
}
