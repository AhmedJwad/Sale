using Sale.Common.Entities;
using Sale.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Helpers
{
   public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, Guid ImageId, bool iSNew);
        CategoryViewModel ToCategoryView(Category category);
    }
}
