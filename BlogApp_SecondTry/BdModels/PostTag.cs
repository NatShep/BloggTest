using Microsoft.EntityFrameworkCore;

namespace BlogApp_SecondTry.BdModels
{
    public class PostTag
    {
        public virtual int PostId { get; set; }
        public Post Post { get; set; }

        public virtual int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
