using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp_SecondTry.BdModels;
using BlogApp_SecondTry.Services;

namespace BlogApp_SecondTry.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostService _postService;

        public HomeController(PostService postService)
        {
            _postService = postService;
        }

        // GET: Posts
        [HttpGet]
        public async Task<IActionResult> Get(int? tagId)
        {
            var query = await _postService.GetAllWithTags(showHidden:false,tagId:tagId);
         
            return View(query);
        }      

        public async Task<IActionResult> GetDetails(int postId) 
            => View(await _postService.GetOrNullWithTagsBy(postId));

    }
}
