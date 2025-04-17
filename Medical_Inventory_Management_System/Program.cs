using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Mappings;
using Medical_Inventory_Management_System.Repositories.Implementations;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services;
using Medical_Inventory_Management_System.Services.Implementations;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllers();

builder.Services.AddScoped<InvoiceService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Database 

builder.Services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("DbCon")));

// Register Services

builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
builder.Services.AddScoped<IManufacturersRepository, ManufacturersRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IBrandsService, BrandsService>();
builder.Services.AddScoped<IManufacturersService, ManufacturersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<InvoiceService>();



// Added Auto Mapping

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
