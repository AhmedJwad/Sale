using Microsoft.AspNetCore.Http;
using Sale.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sale.web.Models
{
    public class CategoryViewModel:Category
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

    }
}
