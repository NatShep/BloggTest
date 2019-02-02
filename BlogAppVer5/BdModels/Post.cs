using System;
using System.Collections.Generic;

namespace BlogAppVer5.BdModels
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public List<PostTag> PostTags { get; set; }

        public Post()
        {
            PostTags = new List<PostTag>();
        }
    }
}
