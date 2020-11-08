using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MVCBlog.ViewModels;
using MVCBlog.Models;
using MVCBlog.Enums;
using MVCBlog.Data;

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
            

        //}
        //public static string DecodeImage(byte[] imageData, string fileName)
        //{
            var binary = Convert.ToBase64String(imageBytes);
            var ext = Path.GetExtension(post.FileName);
            post.ImageDataUrl = $"data:image/{ext};base64,{binary}";
            //string imageDataUrl = $"data:image/{ext};base64,{binary}";
            //return imageDataUrl;
        }
    }
}
