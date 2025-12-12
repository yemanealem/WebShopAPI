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

    // GET: api/customers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Customers.ToListAsync());
    }

    // POST: api/customers
    [HttpPost]
    public async Task<IActionResult> Create(CustomerDTO dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return Ok(customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerDTO dto)
    {
        var customer = await _db.Customers.FindAsync(id);
        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        customer.Name = dto.Name;
        customer.Email = dto.Email;

        await _db.SaveChangesAsync();
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _db.Customers.FindAsync(id);
        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Customer deleted successfully" });
    }
}
