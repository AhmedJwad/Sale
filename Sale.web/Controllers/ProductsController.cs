using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Sale.Common.Entities;
using Sale.web.Data;
using Sale.web.Helpers;
using Sale.web.Models;

namespace Sale.web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public ProductsController(DataContext context, ICombosHelper combosHelper,
            IConverterHelper converterHelper, IBlobHelper blobHelper)
        {
           _context = context;
           _combosHelper = combosHelper;
            _converterHelper = converterHelper;
           _blobHelper = blobHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View( await _context.Products.Include(p=>p.Category).Include(p=>p.ProductImages).ToArrayAsync());
        }

        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel()
            {
                Categories = _combosHelper.GetComboCategories(),
                IsActive = true,

        };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Product product = await _converterHelper.ToProductAsync(model, true);
                    if(model.ImageFile!=null)
                    {
                        Guid ImageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        product.ProductImages = new List<ProductImage>
                        {
                            new ProductImage{ImageId=ImageId}
                        };
                    }
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            Product product = await _context.Products.Include(p => p.Category)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if(product==null)
            {
                return NotFound();
            }
            ProductViewModel model = _converterHelper.ToProductViewModel(product);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _converterHelper.ToProductAsync(model, false);
                   
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        if (product.ProductImages == null)
                        {
                            product.ProductImages = new List<ProductImage>();
                        }
                        product.ProductImages.Add(new ProductImage { ImageId = imageId });
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
               
            }
            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Product product = await _context.Products.Include(p => p.Category)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if(product==null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _context.Products.FindAsync(id);
            AddProductImageViewModel model = new AddProductImageViewModel
            {
                ProductId = product.Id,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if(ModelState.IsValid)
            {
                Product product = await _context.Products
                                          .Include(p => p.ProductImages)
                                          .FirstOrDefaultAsync(p => p.Id == model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
                try
                {
                    Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    if (product.ProductImages == null)
                    {
                        product.ProductImages = new List<ProductImage>();
                    }

                    product.ProductImages.Add(new ProductImage { ImageId = imageId });
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{product.Id}");

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            ProductImage productImage = await _context.productImages.FirstOrDefaultAsync(pi => pi.Id == id);
            if(productImage==null)
            {
                return NotFound();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ProductImages
              .FirstOrDefault(pi => pi.Id == productImage.Id) != null);
            _context.productImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{product.Id}");
        }
    }
}
