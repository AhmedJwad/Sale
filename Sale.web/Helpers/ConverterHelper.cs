using Sale.Common.Entities;
using Sale.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Category ToCategory(CategoryViewModel model, Guid ImageId, bool iSNew)
        {
            return new Category
            {
                Id = iSNew ? 0 : model.Id,
                ImageId =ImageId,
                Name = model.Name,
            };
        }

        public CategoryViewModel ToCategoryView(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                ImageId = category.ImageId,
                Name = category.Name
            };
        }
    }
}
