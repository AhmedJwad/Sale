using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sale.Common.Responses;
using Sale.web.Data;
using Sale.web.Data.Entities;
using Sale.web.Helpers;

namespace Sale.web.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrdersController(DataContext context, IUserHelper userHelper )
        {
           _context = context;
            _userHelper = userHelper;
        }
        [HttpGet]
        public async Task<IActionResult>GetOrders()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user =await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            List<Order> orders = await _context.Orders.Include(o => o.User)
                .ThenInclude(u => u.City).Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(o => o.User.Id == user.Id)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderResponse request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            Order order = new Order
            {
                Date=DateTime.UtcNow,
                OrderDetails=new List<OrderDetail>(),
                OrderStatus=request.OrderStatus,
                PaymentMethod=request.PaymentMethod,
                Remarks=request.Remarks,
                User=user,
            };
            foreach (OrderDetailResponse  item in request.OrderDetails)
            {
                Product product = await _context.Products.FindAsync(item.Product.Id);
                if(product==null)
                {
                    return NotFound("Error002");
                }

                order.OrderDetails.Add(new OrderDetail
                {
                    Price=product.Price,
                    Product=product,
                    Quantity=request.Quantity,
                    Remarks=request.Remarks,
                });
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
        [HttpPut]
        public async Task<IActionResult> PutOrders([FromBody] Order order)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if(user==null)
            {
                return NotFound("Error001");
            }
            Order currentOrder= await _context.Orders.Include(o => o.User)
                .ThenInclude(u => u.City)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(o => o.Id == order.Id && o.User.Id == user.Id);
            if(currentOrder ==null)
            {
                return NotFound();
            }
            currentOrder.OrderStatus = order.OrderStatus;
            currentOrder.Remarks = order.Remarks;
            _context.Orders.Update(currentOrder);
            await _context.SaveChangesAsync();
            return Ok(currentOrder);
        }
    }
}
