using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.Include(s => s.Product).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }
    }
}
