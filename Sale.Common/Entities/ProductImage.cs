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
        public string ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => string.IsNullOrEmpty(ImageId)
            ? $"http://onsaleahmed.somee.com/images/noimage.png"
            : $"http://onsaleahmed.somee.com/{ImageId.Substring(1)}";

    }
}
