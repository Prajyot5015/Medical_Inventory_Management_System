using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AppDbContext _context;

    public PurchaseOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder> AddAsync(PurchaseOrder order)
    {
        _context.PurchaseOrders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in order.Items)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

            if (stock != null)
            {
                stock.CurrentStock += item.Quantity;
                _context.Stocks.Update(stock);
            }
            else
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                var newStock = new Stock
                {
                    ProductId = item.ProductId,
                    CurrentStock = item.Quantity,
                    LowStockThreshold = 10, 
                    ExpiryDate = product?.ExpiryDate 
                };

                await _context.Stocks.AddAsync(newStock);
            }
        }
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<List<PurchaseOrder>> GetPurchaseOrderAsync()
    {
        return await _context.PurchaseOrders
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
    }
}

