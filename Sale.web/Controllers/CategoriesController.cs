using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sale.Common.Entities;
using Sale.web.Data;
using Sale.web.Helpers;
using Sale.web.Models;

namespace Sale.web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public CategoriesController(DataContext context, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _context = context;
           _blobHelper = blobHelper;
           _converterHelper = converterHelper;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if(model.ImageFile!=null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                }

                try
                {
                    Category category =_converterHelper.ToCategory(model, imageId, true);
                    _context.Add(category);
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

            return View(model);

        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel model = _converterHelper.ToCategoryView(category);
            return View(model);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  CategoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            Guid imageId = model.ImageId;
            if(model.ImageFile!=null)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Category category = _converterHelper.ToCategory(model, imageId, false);
                    _context.Update(category);
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


            return View(model);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }    
     
       
    }
}
