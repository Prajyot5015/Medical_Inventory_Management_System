using AutoMapper;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _repository;
        private readonly IMapper _mapper;

        public StockService(IStockRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<StockDto>> GetAllStockDetailsAsync()
        {
            var stocks = await _repository.GetAllStocksAsync();
            return _mapper.Map<List<StockDto>>(stocks);
        }

        public async Task<List<StockDto>> GetLowStockAsync()
        {
            var stocks = await _repository.GetAllStocksAsync();
            var filtered = stocks.Where(s => s.CurrentStock < s.LowStockThreshold).ToList();
            return _mapper.Map<List<StockDto>>(filtered);
        }

        public async Task<List<StockDto>> GetNearExpiryStockAsync()
        {
            var stocks = await _repository.GetAllStocksAsync();
            var filtered = stocks.Where(s =>
                s.ExpiryDate.HasValue &&
                s.ExpiryDate.Value <= DateTime.UtcNow.AddDays(7)
            ).ToList();

            return _mapper.Map<List<StockDto>>(filtered);
        }
        public async Task UpdateStockAfterSaleAsync(int productId, int quantitySold)
        {
            var stock = await _repository.GetAllStocksAsync();
            var productStock = stock.FirstOrDefault(s => s.ProductId == productId);

            if (productStock != null)
            {
                if (productStock.CurrentStock >= quantitySold)
                {
                    productStock.CurrentStock -= quantitySold;
                    var product = productStock.Product;
                    product.DecreaseStock(quantitySold);
                    await _repository.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Insufficient stock");
                }
            }
        }

        public async Task UpdateStockAfterPurchaseAsync(int productId, int quantityPurchased)
        {
            var stock = await _repository.GetAllStocksAsync();
            var productStock = stock.FirstOrDefault(s => s.ProductId == productId);

            if (productStock != null)
            {
                productStock.CurrentStock += quantityPurchased;
                var product = productStock.Product;
                product.IncreaseStock(quantityPurchased);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task AddStockToProductAsync(int productId, int quantityToAdd)
        {
            var stock = await _repository.GetAllStocksAsync();
            var productStock = stock.FirstOrDefault(s => s.ProductId == productId);

            if (productStock != null)
            {
                productStock.CurrentStock += quantityToAdd;
                var product = productStock.Product;
                product.IncreaseStock(quantityToAdd);

                await _repository.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Product not found in stock");
            }
        }
    }
}

