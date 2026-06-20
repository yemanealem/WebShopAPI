public class OrderResponseDTO
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string CustomerPhone { get; set; } = "";

    public string ShippingName { get; set; } = "";
    public string ShippingAddress { get; set; } = "";
    public string ShippingCity { get; set; } = "";
    public string ShippingPostal { get; set; } = "";
    public string ShippingCountry { get; set; } = "";

    public string CardName { get; set; } = "";
    public string CardNumber { get; set; } = "";
    public string CardExpiry { get; set; } = "";
    public string CardCVC { get; set; } = "";

    public List<OrderItemResponseDTO> Items { get; set; } = new();
}