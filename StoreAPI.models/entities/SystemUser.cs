namespace StoreAPI.models.entities;

public class SystemUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public List<Order> Orders { get; set; }
        
}