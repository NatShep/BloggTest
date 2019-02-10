using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp_SecondTry.BdModels;
using Microsoft.AspNetCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace BlogApp_SecondTry.Services
{
    public class PostService
    {
        private readonly BlogContext _blogContext;

        public PostService(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public async Task<List<Post>> GetAllWithTags(bool showHidden, int? tagId=null)
        {
            IQueryable<Post> query =_blogContext.Posts
                .Include(pt => pt.PostTags)
                .ThenInclude(t => t.Tag);
            if(showHidden)
                query = query.Where(q=>q.Published);
            if (tagId != null)
                query = query.Where(q => q.PostTags.Any(t => t.TagId == tagId));
            
             return await query.ToListAsync();
        }

        public async Task<Post> GetOrNullWithTagsBy(int postId) 
            =>  await _blogContext.Posts
                    .Include(pt => pt.PostTags)
                    .ThenInclude(t => t.Tag)
                    .FirstOrDefaultAsync(p => p.Id == postId);

        public async Task<Post> GetOrNullBy(int postId)
            => await _blogContext.Posts.FindAsync(postId);

        public async Task<Post> GetOrNullWithTagsBy(PostTag postTag)
            => await _blogContext.Posts
            .Include(pt => pt.PostTags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(p => p.Id == postTag.PostId);

        public async Task Add(Post post)
        {
            _blogContext.Add(post);
            await _blogContext.SaveChangesAsync();
        }

        public async Task Update(Post post)
        {
            _blogContext.Update(post);
            await _blogContext.SaveChangesAsync();
        }

        public async Task RemoveBy(int id)
        {
            var tag = await _blogContext.Tags.FindAsync(id);
            _blogContext.Tags.Remove(tag);
            await _blogContext.SaveChangesAsync();
        }
        
       
        public async Task AddTagToPost(Post post, int tagId)
        {
            post.PostTags.Add(new PostTag{PostId = post.Id, TagId = tagId});
            await Update(post);
        }

        
        public async Task RemoveTagFromPost(int postId, int tagId)
        {
            var post = _blogContext.Posts.Include(p => p.PostTags).FirstOrDefault(p => p.Id == postId);
            if (post == null)
                return;
            post.PostTags.Remove(post.PostTags.FirstOrDefault(pt => pt.TagId == tagId));
            await _blogContext.SaveChangesAsync();
        }
        
        public bool IsPostExists(int id) 
            => _blogContext.Posts.Any(e => e.Id == id);

            
        
        
        
    }
}