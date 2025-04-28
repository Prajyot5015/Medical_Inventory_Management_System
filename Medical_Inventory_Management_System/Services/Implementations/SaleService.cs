using AutoMapper;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository saleRepo, IMapper mapper)
        {
            _saleRepo = saleRepo;
            _mapper = mapper;
        }
        public async Task<SaleResponseDto> CreateSaleAsync(CreateSaleDto dto)
        {
            decimal totalAmount = 0;

            var sale = new Sale
            {
                CustomerName = dto.CustomerName,
                SaleDate = DateTime.Now,
                Items = new List<SalesItem>()
            };

            foreach (var itemDto in dto.Items)
            {
                var product = await _saleRepo.GetProductByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product ID {itemDto.ProductId} not found");

                if (product.Stock < itemDto.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}");

                product.Stock -= itemDto.Quantity;

                decimal itemTotal = product.Price * itemDto.Quantity;
                totalAmount += itemTotal;

                var saleItem = new SalesItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                sale.Items.Add(saleItem);
                await _saleRepo.UpdateProductAsync(product);
            }

            // Calculate Discount Amount
            decimal discountAmount = totalAmount * (dto.DiscountPercentage / 100);

            sale.TotalAmount = totalAmount;
            sale.Discount = discountAmount;             // store actual discount value
            sale.GrandTotal = totalAmount - discountAmount;

            var savedSale = await _saleRepo.AddSaleAsync(sale);
            return _mapper.Map<SaleResponseDto>(savedSale);
        }


        //public async Task<SaleResponseDto> CreateSaleAsync(CreateSaleDto dto)
        //{
        //    var sale = new Sale
        //    {
        //        CustomerName = dto.CustomerName,
        //        SaleDate = DateTime.Now,
        //        Items = new List<SalesItem>()
        //    };

        //    foreach (var itemDto in dto.Items)
        //    {
        //        var product = await _saleRepo.GetProductByIdAsync(itemDto.ProductId);
        //        if (product == null)
        //            throw new Exception($"Product ID {itemDto.ProductId} not found");

        //        if (product.Stock < itemDto.Quantity)
        //            throw new Exception($"Insufficient stock for {product.Name}");

        //        product.Stock -= itemDto.Quantity;

        //        var saleItem = new SalesItem
        //        {
        //            ProductId = product.Id,
        //            Quantity = itemDto.Quantity,
        //            UnitPrice = product.Price
        //        };

        //        sale.Items.Add(saleItem);
        //        await _saleRepo.UpdateProductAsync(product);
        //    }

        //    var savedSale = await _saleRepo.AddSaleAsync(sale);
        //    return _mapper.Map<SaleResponseDto>(savedSale);
        //}

        public async Task<List<SaleResponseDto>> GetAllSalesAsync()
        {
            var sales = await _saleRepo.GetAllSalesAsync();
            return _mapper.Map<List<SaleResponseDto>>(sales);
        }

        public async Task<SaleResponseDto> GetSaleByIdAsync(int id)
        {
            var sale = await _saleRepo.GetSaleByIdAsync(id);
            if (sale == null)
                throw new Exception("Sale not found");

            return _mapper.Map<SaleResponseDto>(sale);
        }

        public async Task<IEnumerable<SaleResponseDto>> SearchSalesAsync(string query)
        {
            return await _saleRepo.SearchSalesAsync(query);
        }



    }

}
