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
    public class HomeController : Controller
    {
        private readonly BlogContext db;

        public HomeController(BlogContext context)
        {
            db = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? id)
        {
            List<Post> posts;
            if (id == null)
            {
                posts = await db.Posts.Include(pt => pt.PostTags).ThenInclude(t => t.Tag).Where(p => p.Published).ToListAsync();
            }
            else
                posts = await db.Posts.Include(pt => pt.PostTags).ThenInclude(t => t.Tag).Where(p => p.Published).ToListAsync();   //!!!!! как сделать выборку по ид тэгов

            return View(posts);
        }      

        public async Task<IActionResult> PostDetails(int? id)
        {

            return View(await db.Posts.ToListAsync());
        }

        public IActionResult PostTag(int? id)
        {
            return RedirectToAction("Index", "Home"); //!!!! сдtелать вывод по id  { "Square", "Home", id}
        }
    }
}
