using Microsoft.AspNetCore.Http;
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
        public void GetImage(Post post, IFormFile image)
        {
            post.FileName = image.FileName;
            var ms = new MemoryStream();
            image.CopyTo(ms);
            byte[] imageBytes = ms.ToArray();
            ms.Close();
            ms.Dispose();
            

        }
        public static string DecodeImage(byte[] imageData, string fileName)
        {
            var binary = Convert.ToBase64String(imageData);
            var ext = Path.GetExtension(fileName);
            string imageDataUrl = $"data:image/{ext};base64,{binary}";
            return imageDataUrl;
        }
    }
}
