using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App.Repository.Equipment
{
    public interface IEquipmentRep
    {
        Task<IEnumerable<Models.Equipment>> GetEquipments();
        Task CreateEquipment(Models.Equipment supplier);
        Task<Models.Equipment> GetEquipmentToEdit(Guid id);
        Task EditEquipment(Models.Equipment supplier);
        Task DeleteEquipment(Guid id);
    }
}