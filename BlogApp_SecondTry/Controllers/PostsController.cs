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
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        private readonly TagService _tagService;


        public PostsController(PostService postService, TagService tagService)
        {
            _postService = postService;
            _tagService = tagService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _postService.GetAllWithTags(showHidden:true));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetOrNullBy(id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewBag.Date = DateTime.Now;   //!!! cделать так, чтобы время бралось с внешнего сервака
            return View();
            
        }

        // POST: Posts/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreateTime,Body,Published")] Post post)
        {
            if (ModelState.IsValid)
            {

                await _postService.Add(post);    
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id)
        {
            var post =  await _postService.GetOrNullBy(id);
            if (post == null)
                return NotFound();
            return View(post);
        }
        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreateTime,Body,Published")] Post post)
        {
            if (id != post.Id)
                return UnprocessableEntity();
            
            if (!ModelState.IsValid) 
                return View(post);
            if (!_postService.IsPostExists(post.Id))
                return NotFound();
            
            await _postService.Update(post);
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetOrNullBy(id);
            if (post == null)
                return NotFound();
            else    
                return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.RemoveBy(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/EditTag/5
        [HttpGet]
        public async Task<IActionResult> EditTag(int postId)
        {
            var post = await _postService.GetOrNullWithTagsBy(postId); 
            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpGet("GetAllowed/{postId}")]
        public async Task<IActionResult> GetAllowedTags(int postId)
        {
            var post = await _postService.GetOrNullWithTagsBy(postId);

            if (post == null)
                return NotFound();

            var tags = await _tagService.GetAll();
            
            ViewBag.Tags = new SelectList(tags, "Id", "TextTag");
            ViewBag.Post = post;

            return View();
        }
        // Post: Posts/AddTag/5
        [HttpPost]
        public async Task<IActionResult> AddTag(PostTag postTag)  //!!!! почему тут приходит значения только для Id, для самих Post и Tag - значение -Null. Но все выходит ок
        {
            var post = await _postService.GetOrNullWithTagsBy(postTag);

            if (_postService.IsPostExists(post.Id))
                 return RedirectToAction(nameof(Index));

            await _postService.AddTagToPost(post, postTag.TagId);
            return RedirectToAction(nameof(Index));
        }
        // GET: Posts/DeleteTag/
        public async Task<IActionResult> DeleteTag(int tagId, int postId)
        {
            var tag = await _tagService.GetOrNullBy(tagId);
            if (tag==null)
                return NotFound();

            ViewBag.post = await _postService.GetOrNullBy(postId);;
            ViewBag.tag = tag;
            
            return View();
        }
        // POST: Posts/DeleteTag/
        [HttpPost, ActionName("DeleteTag")]
        public async Task<IActionResult> DeleteTagConfirmed(PostTag postTag) 
        {
            await _postService.RemoveTagFromPost(
                postId: postTag.PostId, 
                tagId:  postTag.TagId);
            return RedirectToAction(nameof(Index));
        }
    }
}
