using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sale.web.Data;
using Sale.web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            List<Product> products = await _context.Products.Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Qualifications)
                .Where(p => p.IsActive)
               .ToListAsync();
            return Ok(products);
        }
    }
}
