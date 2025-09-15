namespace StoreAPI.models.DTOS;

public class OrderCDTO
{
    public double Total {get; set;}
    public int SystemUserId {get; set;}
    public List<int> Products { get; set; }
    
}