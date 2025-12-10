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
public async Task<IActionResult> GetAll(string? search = "", int page = 1, int pageSize = 10)
{
    if (page <= 0) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // BASE QUERY
    IQueryable<Product> query = _db.Products;

    // APPLY SEARCH
    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.ToLower();

        query = query.Where(p =>
            p.Title.ToLower().Contains(search) ||
            p.Description.ToLower().Contains(search) ||
            p.Category.ToLower().Contains(search)
        );
    }

    // **COUNT AFTER FILTERING**
    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    // PAGINATION
    var products = await query
        .OrderBy(p => p.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var response = new
    {
        page,
        pageSize,
        totalItems,
        totalPages,
        items = products
    };

    return Ok(response);
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
            RatingRate=5,
            RatingCount=4,
            Image = imageUrl
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return Ok(product);
    }




 [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDTO dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Title = dto.Title;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.Category = dto.Category;

        if (dto.Image != null && dto.Image.Length > 0)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.Image))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath, "images", "products", Path.GetFileName(product.Image));
                if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
            }

            // Save new image
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Image.CopyToAsync(stream);

            var request = HttpContext.Request;
            product.Image = $"{request.Scheme}://{request.Host}/images/products/{fileName}";
        }

        await _db.SaveChangesAsync();
        return Ok(product);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        // Delete image file
        if (!string.IsNullOrEmpty(product.Image))
        {
            var filePath = Path.Combine(_env.WebRootPath, "images", "products", Path.GetFileName(product.Image));
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }


[HttpGet("search")]
public async Task<IActionResult> SearchProduct(string? query = "")
{
    if (string.IsNullOrWhiteSpace(query))
        return BadRequest(new { message = "Query cannot be empty." });

    query = query.ToLower();

    var results = await _db.Products
        .Where(p =>
            p.Title.ToLower().Contains(query) ||
            p.Description.ToLower().Contains(query) ||
            p.Category.ToLower().Contains(query))
        .OrderBy(p => p.Title)
        .ToListAsync();

    return Ok(new
    {
        totalItems = results.Count,
        items = results
    });
}

}









