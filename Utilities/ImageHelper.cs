using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MVCBlog.Models;

namespace MVCBlog.Utilities
{
    public class ImageHelper
    {
        public void TheImageHelper(Post post, IFormFile image)
        {
            post.FileName = image.FileName;
            var ms = new MemoryStream();
            image.CopyTo(ms);
            byte[] imageBytes = ms.ToArray();
            //post.Image = ms.ToArray();
            ms.Close();
            ms.Dispose();
            //var binary = Convert.ToBase64String(post.Image);
            var binary = Convert.ToBase64String(imageBytes);
            var ext = Path.GetExtension(post.FileName);
            post.ImageDataUrl = $"data:image/{ext};base64,{binary}";
        }
    }
}
