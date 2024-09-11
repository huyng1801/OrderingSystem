using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OrderingSystemAPI.Middleware;
using OrderingSystemAPI.Services;
using OrderingSystemData.Models;
using OrderingSystemService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderingSystemContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TableService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});
app.UseAuthorization();

app.MapControllers();

app.Run();
