using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.models.DTOS;
using StoreAPI.models.entities;

namespace FrameworkYConexionBD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        private readonly StoreDbCOntext _context;

        public InvoiceController(StoreDbCOntext context)
        {
            _context = context;

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
    }
}
