using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class ManufacturersRepository : IManufacturersRepository
    {
        private readonly AppDbContext appContext;

        public ManufacturersRepository(AppDbContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<bool> AddManufacturersAsync(Manufacturer manufacturer)
        {
            await appContext.Manufacturers.AddAsync(manufacturer);
            await appContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteManufacturer(int id)
        {
            var manufacturer = appContext.Manufacturers.Find(id);
            if (manufacturer == null) return false;
            appContext.Manufacturers.Remove(manufacturer);
            await appContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            var manufacturers = await appContext.Manufacturers.ToListAsync();
            return manufacturers;
        }

        public async Task<Manufacturer> GetManuFacturerById(int id)
        {
            return await appContext.Manufacturers.FindAsync(id);
        }

        public async Task<bool> UpdateManufacturerAsync(int id, Manufacturer manufacturer)
        {
            var Existingmanufacturer = await appContext.Manufacturers.FindAsync(id);
            if (Existingmanufacturer == null) return false;

            Existingmanufacturer.Name = manufacturer.Name;
            Existingmanufacturer.ContactDetails = manufacturer.ContactDetails;
            Existingmanufacturer.Address = manufacturer.Address;

            appContext.Manufacturers.Update(Existingmanufacturer);
            await appContext.SaveChangesAsync();
            return true;
        }
    }
}
