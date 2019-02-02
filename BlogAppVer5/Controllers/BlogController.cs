using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAppVer5.BdModels;
using Microsoft.EntityFrameworkCore;

namespace BlogAppVer5.Controllers
{
    public class BlogController : Controller
    {
        private BlogContext db;
        public BlogController(BlogContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Posts.Include(t => t.PostTags).ThenInclude(pt=>pt.Tag).ToList());
        }

        public IActionResult About()
        {
            return View();
        }
    }
}