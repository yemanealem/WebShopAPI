using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.DTOs;
using WebShopAPI.Models;

namespace WebShopAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly WebShopDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(WebShopDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductCreateDTO dto)
    {
        string? imageUrl = null;

        if (dto.Image != null && dto.Image.Length > 0)
        {
            // Ensure the folder exists
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            // Build full URL
            var request = HttpContext.Request;
            imageUrl = $"{request.Scheme}://{request.Host}/images/products/{fileName}";
        }

        var product = new Product
        {
            Title = dto.Title,
            Price = dto.Price,
            Description = dto.Description,
            Category = dto.Category,
            Image = imageUrl
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return Ok(product);
    }
}
