namespace StoreAPI.models.entities;

public class Order
{
    public int Id { get; set; }
    public double Total { get; set; }
    public DateTime CreatAt { get; set; }
    public int SystemUserId { get; set; }
    public SystemUser SystemUser { get; set; }
    
    public List<OrderProduct> OrderProducts { get; set; }
}