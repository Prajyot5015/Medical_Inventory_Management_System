using Medical_Inventory_Management_System.Models.Domain;

namespace Medical_Inventory_Management_System.Repositories.Interface
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task SaveChangesAsync();
        Task AddStockAsync(Stock stock);

    }
}
