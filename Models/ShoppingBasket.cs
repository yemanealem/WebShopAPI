namespace WebShopAPI.Models;

public class ShoppingBasket
{
    public int Id { get; set; }

    // Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // Shipping info
    public string ShippingName { get; set; } = "";
    public string ShippingAddress { get; set; } = "";
    public string ShippingCity { get; set; } = "";
    public string ShippingPostal { get; set; } = "";
    public string ShippingCountry { get; set; } = "";

    // Payment info
    public string CardName { get; set; } = "";
    public string CardNumber { get; set; } = "";
    public string CardExpiry { get; set; } = "";
    public string CardCVC { get; set; } = "";

    // Items under this order
    public List<OrderItem> OrderItems { get; set; } = new();
}
