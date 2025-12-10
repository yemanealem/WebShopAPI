using Microsoft.AspNetCore.Http;

namespace WebShopAPI.DTOs;

public class ProductCreateDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; } // The uploaded image
}
