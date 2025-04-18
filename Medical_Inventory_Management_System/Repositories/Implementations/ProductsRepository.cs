using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IStockService stockService;

        public ProductsRepository(AppDbContext appDbContext, IStockService stockService)
        {
            this.appDbContext = appDbContext;
            this.stockService = stockService;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();

            
            var stock = new Stock
            {
                ProductId = product.Id,
                CurrentStock = product.Stock, 
                LowStockThreshold = 10, 
                ExpiryDate = product.ExpiryDate
            };

            await appDbContext.Stocks.AddAsync(stock);
            await appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var existingProduct = await appDbContext.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return false;
            }
            appDbContext.Products.Remove(existingProduct);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await appDbContext.Products
                                    .Include(p => p.Brand)
                                    .Include(p => p.Manufacturer)
                                    .ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await appDbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await appDbContext.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = product.Name;
            existingProduct.Batch = product.Batch;
            existingProduct.Unit = product.Unit;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.BrandId = product.BrandId;
            existingProduct.ManufacturerId = product.ManufacturerId;

            appDbContext.Products.Update(existingProduct);
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}

