namespace WebShopAPI.DTOs;

public class OrderRequestDTO
{
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string CustomerPhone { get; set; } = "";

    public ShippingDTO Shipping { get; set; } = new();

    public PaymentDTO Payment { get; set; } = new();

    public List<OrderItemDTO> Items { get; set; } = new();
}

public class ShippingDTO
{
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string Postal { get; set; } = "";
    public string Country { get; set; } = "";
}

public class PaymentDTO
{
    public string CardName { get; set; } = "";
    public string CardNumber { get; set; } = "";
    public string Expiry { get; set; } = "";
    public string CVC { get; set; } = "";
}
