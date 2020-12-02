﻿using Sale.Common.Entities;
using Sale.web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sale.web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Starred")]
        public bool IsStarred { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        [DisplayName("Product Images Number")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
            ? $"https://localhost:44384/images/noimage.png"
            : ProductImages.FirstOrDefault().ImageFullPath;
        public ICollection<Qualification> Qualifications { get; set; }
        [Display(Name = "Product Qualifications")]
        public int ProductQualifications => Qualifications == null ? 0 : Qualifications.Count;

        public float Qualification => Qualifications == null || Qualifications.Count == 0 ? 0 :
            Qualifications.Average(Q => Q.Score);
    }
}
