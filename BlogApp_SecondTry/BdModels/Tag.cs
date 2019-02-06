using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogApp_SecondTry.BdModels
{
    public class Tag
    {
        public int Id { get; set; }
        public string TextTag { get; set; }
        public virtual List<PostTag> PostTags { get; set; }

        public Tag()
        {
            PostTags = new List<PostTag>();
        }
    }
}