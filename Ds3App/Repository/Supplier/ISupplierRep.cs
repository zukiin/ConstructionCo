using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Supplier
{
    public interface ISupplierRep
    {
        Task<IEnumerable<Models.Supplier>> GetSuppliers();
        Task CreateSupplier(Models.Supplier supplier);
        Task<Models.Supplier> GetSupplierToEdit(Guid id);
        Task EditSupplier(Models.Supplier supplier);
        Task DeleteSupplier(Guid id);
    }
}
