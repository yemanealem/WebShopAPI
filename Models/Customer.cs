namespace WebShopAPI.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";

    public List<ShoppingBasket> Orders { get; set; } = new();
}
