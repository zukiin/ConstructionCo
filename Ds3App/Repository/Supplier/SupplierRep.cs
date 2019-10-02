using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Supplier
{
    public class SupplierRep : ISupplierRep
    {
        public async Task CreateSupplier(Models.Supplier supplier)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                supplier.Id = Guid.NewGuid();
                context.Supplier.Add(supplier);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSupplier(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Supplier.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditSupplier(Models.Supplier supplier)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Supplier.Find(supplier.Id);
                if(update != null)
                {
                    update.SupplierName = supplier.SupplierName;
                    update.SupplierContact = supplier.SupplierContact;
                    update.SupplierEmail = supplier.SupplierEmail;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Models.Supplier>> GetSuppliers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Supplier.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<Models.Supplier> GetSupplierToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Supplier.FindAsync(id);
            }
        }
    }
}