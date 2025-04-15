﻿using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
           var products = await appDbContext.Products
                                .Include(p => p.Brand)
                                .Include(p => p.Manufacturer).ToListAsync();
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
            return true;
        }
    }
}
