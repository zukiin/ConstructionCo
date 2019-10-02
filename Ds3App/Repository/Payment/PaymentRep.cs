using Ds3App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App.Repository.Payment
{
    public class PaymentRep : IPaymentRep
    {
        public async Task CreatePayment(Guid id)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var contract = context.Contract.Find(id);
                context.Payment.Add(new Models.Payment()
                {
                    Id = Guid.NewGuid(),
                    QuotationReference = contract.QuotationReference,
                    ProjectName = contract.ProjectName,
                    AmountDue = decimal.Parse(contract.ProjectCost),
                    Status = Constant.MagicStringReplacer.PendingStatus,
                    ContractId = id,
                    ClientId = contract.ClientId
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task<Models.Payment> GetPayment(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Payment.FindAsync(id);
            }
        }

        public async Task<IEnumerable<Models.Payment>> GetPaymentByClient(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var models = await context.Payment.Where(p => !p.IsDeleted && p.ClientId == userId).ToListAsync();
                foreach (var model in models)
                {
                    if (context.Transactions.Any(t => t.REFERENCE == model.QuotationReference && t.TRANSACTION_STATUS == "1"))
                    {
                        model.Status = Constant.MagicStringReplacer.PaidStatus;
                        model.IsOnlinePayment = true;
                    }
                }
                context.SaveChanges();
                return models;
            }
        }

        public async Task<IEnumerable<Models.Payment>> GetPaymentsByAdmin()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Payment.Where(p => !p.IsDeleted).ToListAsync();

            }
        }

        public async Task UploadProofOfPayment(Models.Payment payment)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var model = await context.Payment.FindAsync(payment.Id);
                model.ProofOfPayment = payment.ProofOfPayment;
                model.Status = Constant.MagicStringReplacer.PaidStatus;
                await context.SaveChangesAsync();
            }
        }
    }
}