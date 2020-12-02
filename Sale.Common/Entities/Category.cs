using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sale.Common.Entities
{
   public  class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImageId { get; set; }       

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath =>string.IsNullOrEmpty(ImageId)
            ? $"http://onsaleahmed.somee.com/images/noimage.png"
            : $"http://onsaleahmed.somee.com/{ImageId.Substring(1)}";

    }
}
