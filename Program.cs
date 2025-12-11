using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlite("Data Source=webshop.db"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseStaticFiles();
app.UseCors("AllowReactApp");


// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
