using Microsoft.EntityFrameworkCore;
using WebShopAPI.Models;

namespace WebShopAPI.Data;

public class WebShopDbContext : DbContext
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) {}

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ShoppingBasket> ShoppingBaskets => Set<ShoppingBasket>();
     public DbSet<OrderItem> OrderItems { get; set; }

}
