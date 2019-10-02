using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Payment
{
    public interface IPaymentRep
    {
        Task CreatePayment(Guid id);
        Task<IEnumerable<Models.Payment>> GetPaymentByClient(string userId);
        Task<Models.Payment> GetPayment(Guid id);
        Task UploadProofOfPayment(Models.Payment payment);
        Task<IEnumerable<Models.Payment>> GetPaymentsByAdmin();
    }
}
