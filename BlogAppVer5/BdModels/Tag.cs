using System;
using System.Collections.Generic;

namespace BlogAppVer5.BdModels
{
    public class Tag
    {
        public int Id { get; set; }
        public string TextTag { get; set; }
        public List<PostTag> PostTags { get; set; }

        public Tag()
        {
            PostTags = new List<PostTag>();
        }
    }
}
