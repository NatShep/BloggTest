using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogApp_SecondTry.BdModels
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public virtual List<PostTag> PostTags { get; set; }

        public Post()
        {
            PostTags = new List<PostTag>();
        }
    }
}
