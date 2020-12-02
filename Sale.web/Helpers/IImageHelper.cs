using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Helpers
{
   public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
        string UploadImage(byte[] pictureArray, string folder);
    }
}
