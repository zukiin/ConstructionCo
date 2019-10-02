using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Client
{
    public interface IClientRep
    {
        Task CreateClient(Models.Client client);
    }
}
