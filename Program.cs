using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Add EF Core with SQLite
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlite("Data Source=webshop.db"));

// Enable Controllers
builder.Services.AddControllers();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
