using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using StoreAPI.models.DTOS;
using StoreAPI.models.entities;

namespace FrameworkYConexionBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbCOntext _context;
        private readonly IConfiguration _config;

        public OrderController(StoreDbCOntext context, IConfiguration config)
        {
            _context = context;
            _config = config;

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
            var orders = await _context.Order
                .Include(o=>o.SystemUser)
                .Select(o => new
                { 
                    Id = o.Id,
                    Total = o.Total,
                    CreatedAt = o.CreatAt,
                    User = new UserDTo
                    {
                        Id = o.SystemUser.Id,
                        Email = o.SystemUser.Email,
                        FirstName = o.SystemUser.FirstName,
                        LastName = o.SystemUser.LastName,
                    }
                })
                .ToListAsync();
    
            // _context.Order.FirstOrDefaultAsync(o=>o.Id == id);
            return Ok(orders);
        }
        
        

        [HttpPost("bulk")]
        public async Task<ActionResult> CreateOrderBulk([FromBody] List<OrderCDTO> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return BadRequest("No se recibi3eorn ordenes");
            }
            
            //Si muevo muchas cosas en grandes cantidaddes siempre tienes que ahcer transacciones
            
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newOrders = orders
                    .Select(o => new Order()
                        {
                            SystemUserId = o.SystemUserId,
                            CreatAt = DateTime.Now,
                            Total = o.Total,
                            OrderProducts = o.Products
                                .Select(op=> new OrderProduct(){Amount = 1, ProductId = op})
                                .ToList()
                            
                        
                        }
                    ).ToList();
                _context.Order.AddRange(newOrders);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok("Ordenes agregadas");
            }
            catch (Exception ex)
            {
                transaction.RollbackAsync();
                throw ex;
            }
            
        }

        [HttpGet("ai-analyze")]
        public async Task<ActionResult> AnalyzeOrders(int id)
        {
            //obtener api key
            var openAIKey = _config["OpenAIKey"];
            var client = new ChatClient(model:"gpt-5-mini",apiKey: openAIKey);


           
            //obtienene datos
            var orders = await _context.Order
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Store)
                .ToListAsync();
            var summary = orders.Select(o => new
            {
                o.Id,
                o.Total,
                o.CreatAt,
                Products = o.OrderProducts.Select(op => new
                {
                    op.Product.Name,
                    op.Product.Precio,
                    op.Product.Store.Description
                })
            });
            var jsonData = JsonSerializer.Serialize(summary);
            //se hace el prompt
            var prompt = Prompts.GenerateOrdersPrompt(jsonData);
            
            var result = await client.CompleteChatAsync(
                new UserChatMessage(prompt));
            var response = result.Value.Content[0].Text;
            return Ok(response);
        }
        
        


        

    }
    
    
}
