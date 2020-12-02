using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }   
        
        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public string ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => string.IsNullOrEmpty(ImageId)
            ? $"http://onsaleahmed.somee.com/images/noimage.png"
            : $"http://onsaleahmed.somee.com/{ImageId.Substring(1)}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [Display(Name = "Department")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a department.")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        [Required]
        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

    }
}
