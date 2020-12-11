using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MVCBlog.Utilities
{
    public class ImageHelper
    {
        public byte[] EncodeImage(IFormFile image)
        {
            var ms = new MemoryStream();
            image.CopyTo(ms);
            var output = ms.ToArray();

            ms.Close();
            ms.Dispose();

            return output;
        }

        public string DecodeImage(byte[] image, string fileName)
        {
            var binary = Convert.ToBase64String(image);
            var ext = Path.GetExtension(fileName);
            string imageDataURL = $"data:image/{ext};base64,{binary}";

            return imageDataURL;
        }
    }
}

//    public class ImageHelper
//    {
//        public void WriteImage(Post post, IFormFile image)
//        {
//            post.FileName = image.FileName;
//            var ms = new MemoryStream();
//            image.CopyTo(ms);
//            byte[] imageBytes = ms.ToArray();
//            //post.Image = ms.ToArray();
//            ms.Close();
//            ms.Dispose();
//            //var binary = Convert.ToBase64String(post.Image);
//            var binary = Convert.ToBase64String(imageBytes);
//            var ext = Path.GetExtension(post.FileName);
//            post.ImageDataUrl = $"data:image/{ext};base64,{binary}";
//        }
//    }
//}