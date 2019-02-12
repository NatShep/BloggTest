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
    public class TagsController : Controller
    {
        private readonly TagService _tagService;

        
        public TagsController(TagService tagService)
        {
            _tagService = tagService;
        }
       
        public async Task<IActionResult> Index() 
            => View(await _tagService.GetAllWithTags() );


        public IActionResult Create() 
            => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TextTag")] Tag tag)
        {
            if (!ModelState.IsValid) 
                return View(tag);
            
            await _tagService.Add(tag);
            return RedirectToAction(nameof(Index));
        }               
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagService.GetOrNullBy(id);

            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tagService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
