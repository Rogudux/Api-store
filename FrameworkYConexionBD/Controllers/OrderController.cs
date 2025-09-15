using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.models.DTOS;
using StoreAPI.models.entities;

namespace FrameworkYConexionBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbCOntext _context;

        public OrderController(StoreDbCOntext context)
        {
            _context = context;

        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(
            [FromBody] OrderCDTO order)
        {
            
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newOrder = new Order()
                {
                    SystemUserId = order.SystemUserId,
                    Total = order.Total,
                    CreatAt = DateTime.Now
                };

                _context.Order.Add(newOrder);
                await _context.SaveChangesAsync();
                
                var orderProducts = order.Products
                    .Select(x=> new OrderProduct{ OrderId = newOrder.Id, ProductId = x, Amount = 3})
                    .ToList();
                    _context.OrderProduct.AddRange(orderProducts);
                
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                
                    return Ok();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Problem(ex.Message);

            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders =await _context.Order
                .Include(o => o.SystemUser)
                .ToListAsync();
            return Ok(orders);
        }

    }
    
    
}
