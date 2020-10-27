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

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }

        public string Slug { get; set; }

        
        public string Content { get; set; }
        public byte[] Image { get; set; }
         
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        // In Microsoft doc this is public type type

        public bool IsPublished { get; set; }

        public virtual Blog Blog { get; set; }
        //In Microsoft doc this is public  list<Type> Types
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        //public List<Comment>  Comments { get; set; }
        // public List<Tag> Tags { get; set; }
        // // var post = new Post();
        public Post()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }

       
        #endregion

        #region Navigation

        #endregion
    }
}
