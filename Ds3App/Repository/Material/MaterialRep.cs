using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Material
{
    public class MaterialRep : IMaterialRep
    {
        public async Task CreateMaterial(Models.Material material)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                material.Id = Guid.NewGuid();
                material.SupplierName = context.Supplier.Find(material.Supplier).SupplierName;
                context.Material.Add(material);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteMaterial(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Material.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditMaterial(Models.Material material)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Material.Find(material.Id);
                if (update != null)
                {
                    update.MaterialImage = material.MaterialImage;
                    update.MaterialName = material.MaterialName;
                    update.MaterialDescription = material.MaterialDescription;
                    update.Price = material.Price;
                    update.StockQuantity = material.StockQuantity;
                    update.Supplier = material.Supplier;
                    update.SupplierName = context.Supplier.Find(material.Supplier).SupplierName;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Models.Material>> GetMaterials()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Material.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<Models.Material> GetMaterialToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Material.FindAsync(id);
            }
        }
    }
}