using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Utilities
{
    public class ImageHelper
    {
        public static string GetImage(Post post)
        {
        var binary = Convert.ToBase64String(post.Image);
        var ext = Path.GetExtension(post.FileName);
        string imageDataURL = $"data:image/{ext};base64,{binary}";
        return imageDataURL;
        }
    }
    
}
