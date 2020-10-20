using Microsoft.AspNetCore.Identity;
using Sale.Common.Entities;
using Sale.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sale.web.Data.Entities
{
    public class User:IdentityUser
    {
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

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44384/images/noimage.png"
            : $"https://onsaleahmed.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public City City { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";    
    }

}

