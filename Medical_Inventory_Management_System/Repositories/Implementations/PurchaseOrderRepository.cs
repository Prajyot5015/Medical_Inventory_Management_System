// Repositories/PurchaseOrderRepository.cs
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

