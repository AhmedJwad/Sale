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
        public Guid ImageId { get; set; }

        public ICollection<Product> Products { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44384/images/noimage.png"
            : $"https://onsaleahmed.blob.core.windows.net/categories/{ImageId}";

    }
}
