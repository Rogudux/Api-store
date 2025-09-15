using System.ComponentModel.DataAnnotations;

namespace StoreAPI.models.DTOS;

public class InvoiceDTO
{
    public int OrderId { get; set; }
    
    [Required]
    public string InvoiceNumber { get; set; }
    
    public double Subtotal { get; set; }
    
    public double Tax { get; set; }
    
    public double Total { get; set; }
    
    [Required]
    [StringLength(3, ErrorMessage = "Currency must be a 3-letter ISO code")] 
    public string Currency { get; set; }
    
    public bool IsPaid { get; set; }
    
    [Required]
    public string BillingName { get; set; }
    
    public string? BillingAddress { get; set; }
    
    [EmailAddress]
    public string? BillingEmail { get; set; }
    
    public string? TaxId { get; set; } 

}