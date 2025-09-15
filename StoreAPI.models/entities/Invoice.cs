using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.models.entities;

public class Invoice
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; }

    [Required]
    public string InvoiceNumber { get; set; } 

    [Required]
    public DateTime IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    public double Subtotal { get; set; }

    public double Tax { get; set; }

    public double Total { get; set; }

    [Required]
    [StringLength(3, ErrorMessage = "Currency must be a 3-letter ISO code")] 
    public string Currency { get; set; }

    public bool IsPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    [Required]
    public string BillingName { get; set; }

    public string? BillingAddress { get; set; }

    [EmailAddress] 
    public string? BillingEmail { get; set; }

    public string? TaxId { get; set; } 

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    
}