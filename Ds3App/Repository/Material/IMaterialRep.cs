using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Material
{
    public interface IMaterialRep
    {
        Task<IEnumerable<Models.Material>> GetMaterials();
        Task CreateMaterial(Models.Material material);
        Task<Models.Material> GetMaterialToEdit(Guid id);
        Task EditMaterial(Models.Material material);
        Task DeleteMaterial(Guid id);
    }
}
