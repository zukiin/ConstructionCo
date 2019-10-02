using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Contract
{
    public interface IContractRep
    {
        Task<Guid> CreateContract(Guid id);
        Task<IEnumerable<Models.Contract>> GetContracts();
        Task<IEnumerable<Models.Contract>> GetMyContracts(string userId);
        Task<Models.Contract> GetContract(Guid id);
        Task Accept(Guid id);
        Task Decline(Guid id);
    }
}
