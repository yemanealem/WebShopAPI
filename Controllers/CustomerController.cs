using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.DTOs;
using WebShopAPI.Models;

namespace WebShopAPI.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly WebShopDbContext _db;

    public CustomerController(WebShopDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Customers.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerDTO dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return Ok(customer);
    }
}
