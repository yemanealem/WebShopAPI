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

    IQueryable<Product> query = _db.Products;

    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.ToLower();

        query = query.Where(p =>
            p.Title.ToLower().Contains(search) ||
            p.Description.ToLower().Contains(search) ||
            p.Category.ToLower().Contains(search)
        );
    }

    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

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
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var request = HttpContext.Request;
            imageUrl = $"{request.Scheme}://{request.Host}/images/products/{fileName}";
        }

        var product = new Product
        {
            Title = dto.Title,
            Price = dto.Price,
            Description = dto.Description,
            Category = dto.Category,
            RatingRate=4.3,
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
        if (!string.IsNullOrEmpty(product.Image))
        {
            var oldFilePath = Path.Combine(_env.WebRootPath, "images", "products", Path.GetFileName(product.Image));
            if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);
        }

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


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

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


[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var product = await _db.Products.FindAsync(id);
    if (product == null)
        return NotFound(new { message = "Product not found." });

    return Ok(product);
}

}









