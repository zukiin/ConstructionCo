using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.Contract
{
    public class ContractRep : IContractRep
    {
        public async Task<IEnumerable<Models.Contract>> GetContracts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Contract.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<IEnumerable<Models.Contract>> GetMyContracts(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Contract.Where(s => !s.IsDeleted && s.ClientId == userId)
                    .ToListAsync();
            }
        }

        public async Task<Guid> CreateContract(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Models.Contract contract = new Models.Contract();
                var quoteRequest = await context.QuotationRequest.FindAsync(id);
                var quote = await context.Quotation.Where(q => q.ReferenceNumber == quoteRequest.ReferenceNumber)
                    .FirstOrDefaultAsync();
                var client = await context.Client.Where(c => c.UserId == quoteRequest.ClientId).FirstOrDefaultAsync();
                contract.Id = Guid.NewGuid();
                contract.ClientName = client.FirstName;
                contract.ClientSurname = client.LastName;
                contract.Email = client.Email;
                contract.Contact = client.Contact;
                contract.QuotationReference = quote.ReferenceNumber;
                contract.ProjectName = quote.ProjectName;
                contract.ProjectCost = quote.EstimatedCost.ToString();
                contract.ProjectCost = quote.EstimatedCost.ToString();
                contract.ContractContent = Constant.MagicStringReplacer.ContractContent;
                contract.DateTimeStamp = DateTime.Now.ToLongDateString();
                contract.ClientId = client.UserId;
                contract.ContractIssuedBy = Constant.MagicStringReplacer.CompanyNameTheIssuer;
                contract.Status = Constant.MagicStringReplacer.PendingStatus;

                context.Contract.Add(contract);
                await context.SaveChangesAsync();
                return contract.Id;
            }
        }

        public async Task<Models.Contract> GetContract(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Contract.FindAsync(id);
            }
        }

        public async Task Accept(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var model = context.Contract.Find(id);
                model.Status = Constant.MagicStringReplacer.AcceptedStatus;

                //Add Payment
                Models.Payment payment = new Models.Payment()
                {
                    Id = Guid.NewGuid(),
                    ProjectName = model.ProjectName,
                    AmountDue = decimal.Parse(model.ProjectCost),
                    Status = Constant.MagicStringReplacer.PendingStatus,
                    ContractId = id,
                    ClientId = model.ClientId,
                    QuotationReference = model.QuotationReference
                };
                context.Payment.Add(payment);
                //
                await context.SaveChangesAsync();
            }
        }

        public async Task Decline(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Contract.Find(id).Status = Constant.MagicStringReplacer.DeclinedStatus;
                await context.SaveChangesAsync();
            }
        }
    }
}