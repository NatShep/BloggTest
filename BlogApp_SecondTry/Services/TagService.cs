using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BlogApp_SecondTry.BdModels;
using Microsoft.AspNetCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace BlogApp_SecondTry.Services
{
    public class TagService
    {
        private readonly BlogContext _blogContext;

        public TagService(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }   
        
        public async Task<List<Tag>> GetAll()
            => await _blogContext.Tags.ToListAsync();

        public async Task<List<Tag>> GetAllWithTags()
        {
            IQueryable<Tag> query =_blogContext.Tags
                .Include(pt => pt.PostTags)
                .ThenInclude(t => t.Post);
           
            return await query.ToListAsync();
        }
        
        public async Task Add(Tag tag)
        {
            _blogContext.Add(tag);
            await _blogContext.SaveChangesAsync();
        }

        public async Task<Tag> GetOrNullBy(int tagId)
            => await _blogContext.Tags.FindAsync(tagId);

        public async Task Remove(int id)
        {
            var tag = await _blogContext.Tags.FindAsync(id);
            _blogContext.Tags.Remove(tag);
            await _blogContext.SaveChangesAsync();
        }
        
        public bool TagExists(int id)
        {
            return _blogContext.Tags.Any(e => e.Id == id);
        }

    }
}