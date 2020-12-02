using Sale.Common.Entities;
using Sale.web.Data.Entities;
using Sale.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Helpers
{
   public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, string ImageId, bool iSNew);
        CategoryViewModel ToCategoryView(Category category);

        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);
        ProductViewModel ToProductViewModel(Product product);
    }
}
