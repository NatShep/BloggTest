using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogAppVer5.Models;
using BlogAppVer5.BdModels;

namespace BlogAppVer5.Controllers
{
    public class HomeController : Controller
    {
        private BlogContext db;
        public HomeController(BlogContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }





        public IActionResult Contact()

        {
            string s = "a";

            Post post1 = new Post
            {
                Body = "Пост со связью с тегом"
            };
            Tag tag = new Tag
            {
                TextTag = "Тестоваяс со связью с блогом"
            };

            

            db.Posts.Add(post1);
            db.Tags.Add(tag);
            db.SaveChanges();

            PostTag pts = new PostTag { TagId = tag.Id, PostId = post1.Id };
            post1.PostTags.Add(pts);
            tag.PostTags.Add(pts); //добавила сама, потому что есть подозрение, что не работает так как на метаните указано
            db.SaveChanges();
                var users = db.Posts.ToList();
                foreach (Post post in users)
                {
                    s = s+ $"{post.Body}";
                }
            s = s + "       -----------------------     /n/r";

            s = s + $"Tags: ";
            var tags = db.Tags.ToList();
            foreach (Tag t in tags)
            {
                s = s + $"{ t.TextTag}"+" ";
            }
            s = s + "      ---------------------------     ";

            s = s + $"Post of Tag: ";
            var posts = tag.PostTags.Select(pt => pt.Post).ToList();
            foreach (Post p in posts)
                s = s + $"{tag.TextTag} => {p.Body}";

                            
           

            ViewData["Message"] = $"база сохранена {s}";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
