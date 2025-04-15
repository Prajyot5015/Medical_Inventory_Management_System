using Medical_Inventory_Management_System.Data;
using Medical_Inventory_Management_System.Models.Domain;
using Medical_Inventory_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Medical_Inventory_Management_System.Repositories.Implementations
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly AppDbContext appDbContext;

        public BrandsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<bool> AddBrandAsync(Brand brand)
        {
            try
            {
                await appDbContext.Brands.AddAsync(brand);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding brand: " + ex.Message);
            }
        }
        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            try
            {
                var brands = await appDbContext.Brands.ToListAsync();
                return brands;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving brands: " + ex.Message);
            }
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            try
            {
                var brand = await appDbContext.Brands.FindAsync(id);
                return brand;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving brand: " + ex.Message);
            }
        }

        public async Task<bool> UpdateBrandAsync(int id, Brand brand)
        {
            var existingBrand = appDbContext.Brands.Find(id);
            if (existingBrand == null) return false;

            existingBrand.Name = brand.Name;
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
           var brand = await appDbContext.Brands.FindAsync(id);
            if (brand == null) return false;

            appDbContext.Brands.Remove(brand);
            await appDbContext.SaveChangesAsync();
            return true;
        }

    }
}
