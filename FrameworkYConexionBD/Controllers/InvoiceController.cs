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
    public class InvoiceController : ControllerBase
    {

        private readonly StoreDbCOntext _context;
        private readonly IConfiguration _config;


        public InvoiceController(StoreDbCOntext context, IConfiguration config)
        {
            _context = context;
            _config = config;


        }

        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> GetInvoices()
        {
            var invoices = await _context.Invoice
                .ToListAsync();
            return Ok(invoices);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoicesById(int id)
        {
            var invoiceId = await _context.Invoice
                .FirstOrDefaultAsync(i => i.Id == id);
            return Ok(invoiceId);
        }
        
        
        
        [HttpPost]
        public async Task<ActionResult> CreateInvoice(
            [FromBody] InvoiceDTO invoice)
        {
            
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newInvoice = new Invoice()
                {
                    OrderId = invoice.OrderId,
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    InvoiceNumber =  invoice.InvoiceNumber,
                    Subtotal = invoice.Subtotal,
                    Tax = invoice.Tax,
                    Total = invoice.Total,
                    Currency =  invoice.Currency,
                    IsPaid =  invoice.IsPaid,
                    PaymentDate = DateTime.Now,
                    BillingName = invoice.BillingName,
                    BillingAddress = invoice.BillingAddress,
                    BillingEmail = invoice.BillingEmail,
                    TaxId = invoice.TaxId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                _context.Invoice.Add(newInvoice);
                
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
        
        [HttpGet("ai-analyze")]
        public async Task<ActionResult> AnalyzeInvoices(int id)
        {
            //obtener api key
            var openAIKey = _config["OpenAIKey"];
            var client = new ChatClient(model:"gpt-5-mini",apiKey: openAIKey);
            
           
            //obtienene datos
            
            var invoices = await _context.Invoice
                .ToListAsync();
            var summary = invoices.Select(o => new
            {
                o.BillingAddress,
                o.BillingEmail,
                o.BillingName,
                o.CreatedAt,
                o.DueDate,
                o.Currency,
                o.IsPaid,
                o.PaymentDate,
                o.InvoiceNumber,
                o.Subtotal,
                o.Tax,
                o.Total,
                o.Id,
                o.TaxId
                
            });
            var jsonData = JsonSerializer.Serialize(summary);
            
            //se hace el prompt
            var prompt = Prompts.GenerateInvoicePrompt(jsonData);
            
            var result = await client.CompleteChatAsync(
                new UserChatMessage(prompt));
            var response = result.Value.Content[0].Text;
            return Ok(response);
        }
        
        [HttpPost("bulk")]
        public async Task<ActionResult> CreateInvoiceBulk([FromBody] List<InvoiceDTO> invoices)
        {
            if (invoices == null || invoices.Count == 0)
            {
                return BadRequest("No se recibi3eorn invoices");
            }
            
            //Si muevo muchas cosas en grandes cantidaddes siempre tienes que ahcer transacciones
            
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newInvoices = invoices
                    .Select(o => new Invoice()
                        {
                            OrderId = o.OrderId,
                            IssueDate = DateTime.Now,
                            DueDate = DateTime.Now,
                            InvoiceNumber = o.InvoiceNumber,
                            Subtotal = o.Subtotal,
                            Tax = o.Tax,
                            Total = o.Total,
                            Currency = o.Currency,
                            IsPaid = o.IsPaid,
                            PaymentDate = DateTime.Now,
                            BillingName = o.BillingName,
                            BillingAddress = o.BillingAddress,
                            BillingEmail = o.BillingEmail,
                            TaxId = o.TaxId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        
                        }
                    ).ToList();
                _context.Invoice.AddRange(newInvoices);
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
        
        
    }
}
