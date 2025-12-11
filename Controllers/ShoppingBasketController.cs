using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.DTOs;
using WebShopAPI.Models;

namespace WebShopAPI.Controllers;

[ApiController]
[Route("api/order")]
public class ShoppingBasketController : ControllerBase
{
    private readonly WebShopDbContext _db;

    public ShoppingBasketController(WebShopDbContext db)
    {
        _db = db;
    }

    // GET all orders
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _db.ShoppingBaskets
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ToListAsync();

        return Ok(orders);
    }

    // POST: place order
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(OrderRequestDTO dto)
    {
        // 1️⃣ Find or create customer automatically
        var customer = await _db.Customers
            .FirstOrDefaultAsync(c => c.Email == dto.CustomerEmail || c.Phone == dto.CustomerPhone);

        if (customer == null)
        {
            customer = new Customer
            {
                Name = dto.CustomerName,
                Email = dto.CustomerEmail,
                Phone = dto.CustomerPhone
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        // 2️⃣ Create shopping basket (order)
        var order = new ShoppingBasket
        {
            CustomerId = customer.Id,

            ShippingName = dto.Shipping.Name,
            ShippingAddress = dto.Shipping.Address,
            ShippingCity = dto.Shipping.City,
            ShippingPostal = dto.Shipping.Postal,
            ShippingCountry = dto.Shipping.Country,

            CardName = dto.Payment.CardName,
            CardNumber = dto.Payment.CardNumber,
            CardExpiry = dto.Payment.Expiry,
            CardCVC = dto.Payment.CVC
        };

        _db.ShoppingBaskets.Add(order);
        await _db.SaveChangesAsync(); // Save to get OrderId

        // 3️⃣ Add order items
        foreach (var item in dto.Items)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };

            _db.OrderItems.Add(orderItem);
        }

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Order placed successfully",
            orderId = order.Id,
            customerId = customer.Id
        });
    }
}
