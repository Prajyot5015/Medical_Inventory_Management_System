using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var item in sale.Items)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.CurrentStock -= item.Quantity;

                    if (stock.CurrentStock < 0)
                        stock.CurrentStock = 0; 

                    _context.Stocks.Update(stock);
                }
                else
                {
                    throw new Exception($"No stock found for Product ID {item.ProductId}");
                }
            }

            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            return await _context.Sales.Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

    }

}
