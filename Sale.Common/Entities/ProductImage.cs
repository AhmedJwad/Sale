using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sale.Common.Entities
{
   public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44384/images/noimage.png"
            : $"https://onsale.blob.core.windows.net/products/{ImageId}";

    }
}
