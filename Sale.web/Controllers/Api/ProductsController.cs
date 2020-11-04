using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sale.web.Data;
using System.Linq;

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
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.Include(p => p.Category)
                .Include(p=>p.ProductImages)
                .OrderBy(p=>p.Name));
        }
    }
}
