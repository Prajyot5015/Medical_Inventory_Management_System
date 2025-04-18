﻿using Medical_Inventory_Management_System.Models.DTOs;

namespace Medical_Inventory_Management_System.Services.Interface
{
    public interface IStockService
    {
        Task<List<StockDto>> GetAllStockDetailsAsync();
        Task<List<StockDto>> GetLowStockAsync();
        Task<List<StockDto>> GetNearExpiryStockAsync();

        Task UpdateStockAfterPurchaseAsync(int productId, int quantitySold);

        Task UpdateStockAfterSaleAsync(int productId, int quantitySold);

        Task AddStockToProductAsync(int productId, int quantityToAdd);
    }
}
