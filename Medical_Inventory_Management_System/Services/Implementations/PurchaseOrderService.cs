using AutoMapper;
using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Models.DTOs;
using Medical_Inventory_Management_System.Repositories.Interface;
using Medical_Inventory_Management_System.Services.Interface;

namespace Medical_Inventory_Management_System.Services.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository purchaseOrderRepository;
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public PurchaseOrderService(
            IPurchaseOrderRepository purchaseOrderRepository,
            IMapper mapper,
            AppDbContext context)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<PurchaseOrderDTO> CreateAsync(CreatePurchaseOrderDTO dto)
        {
            var purchaseOrder = mapper.Map<PurchaseOrder>(dto);
            purchaseOrder.OrderDate = DateTime.UtcNow;

            var result = await purchaseOrderRepository.AddAsync(purchaseOrder);

            if (result == null)
                throw new Exception("Failed to create purchase order");

            var purchaseOrderDTO = mapper.Map<PurchaseOrderDTO>(result);

            foreach (var item in purchaseOrderDTO.Items)
            {
                var product = await context.Products.FindAsync(item.ProductId);
                item.ProductName = product?.Name;
            }

            return purchaseOrderDTO;
        }

        public async Task<List<PurchaseOrderDTO>> GetPurchaseOrderAsync()
        {
            var orders = await purchaseOrderRepository.GetPurchaseOrderAsync();

            var orderDTOs = mapper.Map<List<PurchaseOrderDTO>>(orders);

            foreach (var order in orderDTOs)
            {
                foreach (var item in order.Items)
                {
                    var product = await context.Products.FindAsync(item.ProductId);
                    item.ProductName = product?.Name;
                }
            }

            return orderDTOs;
        }
    }
}
