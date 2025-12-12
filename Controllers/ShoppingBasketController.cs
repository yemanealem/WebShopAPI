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

    [HttpGet]
public async Task<IActionResult> GetAll()
{
    var orders = await _db.ShoppingBaskets
        .Include(o => o.Customer)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
        .ToListAsync();

    var orderDtos = orders.Select(o => new OrderResponseDTO
    {
        OrderId = o.Id,
        CustomerName = o.Customer.Name,
        CustomerEmail = o.Customer.Email,
        CustomerPhone = o.Customer.Phone,
        ShippingName = o.ShippingName,
        ShippingAddress = o.ShippingAddress,
        ShippingCity = o.ShippingCity,
        ShippingPostal = o.ShippingPostal,
        ShippingCountry = o.ShippingCountry,
        CardName = o.CardName,
        CardNumber = o.CardNumber,
        CardExpiry = o.CardExpiry,
        CardCVC = o.CardCVC,
        Items = o.OrderItems.Select(oi => new OrderItemResponseDTO
        {
            ProductId = oi.ProductId,
            ProductName = oi.Product.Title,
            Quantity = oi.Quantity,
            Price = oi.Product.Price
        }).ToList()
    }).ToList();

    return Ok(orderDtos);
}

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(OrderRequestDTO dto)
    {
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
        await _db.SaveChangesAsync();

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

      [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _db.ShoppingBaskets
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new { message = "Order not found" });
        }

        _db.OrderItems.RemoveRange(order.OrderItems);

        _db.ShoppingBaskets.Remove(order);

        await _db.SaveChangesAsync();

        return Ok(new { message = "Order deleted successfully" });
    }

     [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _db.ShoppingBaskets
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { message = "Order not found" });

        var orderDto = new OrderResponseDTO
        {
            OrderId = order.Id,
            CustomerName = order.Customer.Name,
            CustomerEmail = order.Customer.Email,
            CustomerPhone = order.Customer.Phone,
            ShippingName = order.ShippingName,
            ShippingAddress = order.ShippingAddress,
            ShippingCity = order.ShippingCity,
            ShippingPostal = order.ShippingPostal,
            ShippingCountry = order.ShippingCountry,
            CardName = order.CardName,
            CardNumber = order.CardNumber,
            CardExpiry = order.CardExpiry,
            CardCVC = order.CardCVC,
            Items = order.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Title,
                Quantity = oi.Quantity,
                Price = oi.Product.Price
            }).ToList()
        };

        return Ok(orderDto);
    }
}
