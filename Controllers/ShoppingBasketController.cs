using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.DTOs;
using WebShopAPI.Models;

namespace WebShopAPI.Controllers;

[ApiController]
[Route("api/basket")]
public class ShoppingBasketController : ControllerBase
{
    private readonly WebShopDbContext _db;

    public ShoppingBasketController(WebShopDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var baskets = await _db.ShoppingBaskets
            .Include(x => x.Product)
            .Include(x => x.Customer)
            .ToListAsync();

        return Ok(baskets);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BasketDTO dto)
    {
        var basket = new ShoppingBasket
        {
            ProductId = dto.ProductId,
            CustomerId = dto.CustomerId,
            Quantity = dto.Quantity
        };

        _db.ShoppingBaskets.Add(basket);
        await _db.SaveChangesAsync();
        return Ok(basket);
    }
}
