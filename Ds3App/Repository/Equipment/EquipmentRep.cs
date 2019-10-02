using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Equipment
{
    public class EquipmentRep : IEquipmentRep
    {
        public async Task CreateEquipment(Models.Equipment equipment)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                equipment.Id = Guid.NewGuid();
                context.Equipment.Add(equipment);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteEquipment(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Equipment.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditEquipment(Models.Equipment equipment)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Equipment.Find(equipment.Id);
                if (update != null)
                {
                    update.EquipmentImage = equipment.EquipmentImage;
                    update.EquipmentName = equipment.EquipmentName;
                    update.Model = equipment.Model;
                    update.Brand = equipment.Brand;
                    update.RatePerHour = equipment.RatePerHour;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Models.Equipment>> GetEquipments()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Equipment.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<Models.Equipment> GetEquipmentToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Equipment.FindAsync(id);
            }
        }
    }
}