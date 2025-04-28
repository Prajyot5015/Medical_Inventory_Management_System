using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
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

        public async Task<IEnumerable<SaleResponseDto>> SearchSalesAsync(string query)
        {
            query = query.ToLower();

            var sales = await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product) 
                .Where(s =>
                    s.CustomerName.ToLower().Contains(query) ||
                    s.TotalAmount.ToString().Contains(query) ||
                    s.Discount.ToString().Contains(query) ||
                    s.GrandTotal.ToString().Contains(query) ||
                    s.SaleDate.ToString().Contains(query)
                )
                .Select(s => new SaleResponseDto
                {
                    Id = s.Id,
                    CustomerName = s.CustomerName,
                    SaleDate = s.SaleDate,
                    TotalAmount = s.TotalAmount,
                    Discount = s.Discount,
                    GrandTotal = s.GrandTotal,
                    Items = s.Items.Select(item => new SaleItemResponseDto
                    {
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                })
                .ToListAsync();

            return sales;
        }

       
    }

}
