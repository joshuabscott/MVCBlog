using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Models
{
    public class Post
    {
        //Keys
        #region Keys 
        public int Id { get; set; }
        public int BlogId { get; set; }
        #endregion

        #region Post Properties
        //Describe the things that a blog post have
       
        public string Title { get; set; }

        //[StringLength(120, MinimumLength =6)]
        public string Abstract { get; set; }

        //[StringLength(500, MinimumLength = 120)]
        public string Body { get; set; }

        public string Slug { get; set; }
        public bool IsPublished { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public byte[] Image { get; set; }
        public string ImageDataUrl { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        // In Microsoft doc this is public type type

        #endregion

        #region Navigation
        public virtual Blog Blog { get; set; }
        //In Microsoft doc this is public  list<Type> Types
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        #endregion
        public Post()
        {
            Comments = new HashSet<Comment>();  //10/20/2020
            Tags = new HashSet<Tag>();
        }  
    }
}
