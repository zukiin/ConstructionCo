using Ds3App.Constant;
using Ds3App.EmailerService;
using Ds3App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ds3App.Repository.Quote
{
    public class QuoteRep : IQuoteRep
    {
        EmailSender email = new EmailSender();
        public async Task<IEnumerable<QuotationRequest>> GetMyQuotes(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.QuotationRequest.Where(q => q.ClientId == userId && !q.IsDeleted).ToListAsync();
            }
        }

        public async Task<IEnumerable<QuotationRequest>> GetQuotes(string search)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return await context.QuotationRequest.Where(q => !q.IsDeleted &&
                    (q.ReferenceNumber.Contains(search)||
                     q.ProjectName.Contains(search)||
                     q.ProjectDescription.Contains(search))).ToListAsync();
                }
                else
                {
                    return await context.QuotationRequest.Where(q => !q.IsDeleted).ToListAsync();
                }
            }
        }

        public async Task RequestQuote(QuotationRequest request)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var newQuoteId = Guid.NewGuid();
                context.QuotationRequest.Add(new QuotationRequest()
                {
                    Id = newQuoteId,
                    ClientId = request.ClientId,
                    ReferenceNumber = request.ReferenceNumber,
                    ProjectName = request.ProjectName,
                    ProjectDescription = request.ProjectDescription,
                    ProjectDocuments = request.ProjectDocuments,
                    DateTimeStamp = DateTime.Now,
                    Status = MagicStringReplacer.PendingStatus,
                });
                //Add equipment
                foreach (var item in context.Equipment.OrderBy(e => e.EquipmentName).ToList())
                {
                    context.QuoteEquipment.Add(new QuoteEquipment()
                    {
                        Id = Guid.NewGuid(),
                        QuotationRequestId = newQuoteId,
                        EquipmentId = item.Id,
                        Quantity = 0,
                        IsAdded = false
                    });
                }
                //end

                //Add material
                foreach (var item in context.Material.OrderBy(e => e.MaterialName).ToList())
                {
                    context.QuoteMaterial.Add(new QuoteMaterial()
                    {
                        Id = Guid.NewGuid(),
                        QuotationRequestId = newQuoteId,
                        MaterialId = item.Id,
                        Quantity = 0,
                        IsAdded = false
                    });
                }
                //
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteRequestQuote(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.QuotationRequest.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<QuotationRequest> GetRequestQuoteToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.QuotationRequest.FindAsync(id);
            }
        }

        public async Task EditRequestQuote(QuotationRequest request)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.QuotationRequest.Find(request.Id);
                if (update != null)
                {
                    update.ProjectName = request.ProjectName;
                    update.ProjectDescription = request.ProjectDescription;
                    update.ProjectDocuments = request.ProjectDocuments;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task CreateQuote(QuotationViewModel quotation)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var q = new Quotation();

                q.Id = Guid.NewGuid();
                q.ReferenceNumber = quotation.ReferenceNumber;
                q.ProjectName = quotation.ProjectName;
                q.ProjectDescription = quotation.ProjectDescription;
                q.ClientId = quotation.ClientId;
                q.Documents = quotation.Documents;
                q.SkilledWorkers = int.Parse(quotation.SkilledWorkers);
                q.SemiSkilledWorkers = int.Parse(quotation.SemiSkilledWorkers);
                q.SSWTaskDescription = quotation.SSWTaskDescription;
                q.SWTaskDescription = quotation.SWTaskDescription;
                q.GWTaskDescription = quotation.GWTaskDescription;

                q.SemiSkilledWorkerTasks = int.Parse(quotation.SemiSkilledWorkerTasks);
                q.SkilledWorkerTasks = int.Parse(quotation.SkilledWorkerTasks);
                q.GeneralWorkerTasks = int.Parse(quotation.GeneralWorkerTasks);
                q.GeneralWorkers = int.Parse(quotation.GeneralWorkers);
                q.StartDate = DateTime.Parse(quotation.StartDate);
                q.EndDate = DateTime.Parse(quotation.EndDate);
                q.WorkWeekends = quotation.WorkWeekends;
                q.Shifts = int.Parse(quotation.Shifts);
                q.ShiftHours = int.Parse(quotation.ShiftHours);
                q.EstimatedCost = await CalcCost(quotation);

                

                var model = context.QuotationRequest.Where(qq => qq.ReferenceNumber == quotation.ReferenceNumber).FirstOrDefault();
                model.IsCompleted = true;
                model.Status = MagicStringReplacer.QuotedStatus;

                context.Quotation.Add(q);
                await context.SaveChangesAsync();
                var toEmail = context.Users.Find(q.ClientId).Email;
                await email.SendQuote(toEmail, q);
            }
        }

        private async Task<decimal> CalcCost(QuotationViewModel quotation)
        {
            decimal cost = 0;
            double hours = 0;
            decimal material = 0;
            decimal equipment = 0;
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var id = context.QuotationRequest.Where(q => q.ReferenceNumber == quotation.ReferenceNumber).FirstOrDefault().Id;
                var tasks = await context.ConstructionTask.Where(t => t.IsDeleted).ToListAsync();

                //get worker ids
                var foreman = context.WorkerType.Where(w => w.Slug == Type.foreman.ToString()).FirstOrDefault().Id;
                var generalworker = context.WorkerType.Where(w => w.Slug == Type.generalworker.ToString()).FirstOrDefault().Id;
                var semiskilledworker = context.WorkerType.Where(w => w.Slug == Type.semiskilledworker.ToString()).FirstOrDefault().Id;
                var skilledworker = context.WorkerType.Where(w => w.Slug == Type.skilledworker.ToString()).FirstOrDefault().Id;
                //end

                //get total hours
                hours = DateTime.Parse(quotation.EndDate).Subtract(DateTime.Parse(quotation.StartDate)).TotalDays * int.Parse(quotation.Shifts) * int.Parse(quotation.ShiftHours);
                //end

                //cost per worker type
                decimal foremanAvRate = 0;
                decimal generalworkerAvRate = 0;
                decimal semiskilledworkerAvRate = 0;
                decimal skilledworkerAvRate = 0;

                var foremanCost = await context.ConstructionTask.Where(c => c.WorkerTypeId == foreman).ToListAsync();
                var generalworkerCost = await context.ConstructionTask.Where(c => c.WorkerTypeId == generalworker).ToListAsync();
                var semiskilledworkerCost = await context.ConstructionTask.Where(c => c.WorkerTypeId == semiskilledworker).ToListAsync();
                var skilledworkerCost = await context.ConstructionTask.Where(c => c.WorkerTypeId == skilledworker).ToListAsync();

                foreach (var item in foremanCost)
                {
                    foremanAvRate += item.RatePerHour;
                }
                foreach (var item in generalworkerCost)
                {
                    generalworkerAvRate += item.RatePerHour;
                }
                foreach (var item in semiskilledworkerCost)
                {
                    semiskilledworkerAvRate += item.RatePerHour;
                }
                foreach (var item in skilledworkerCost)
                {
                    skilledworkerAvRate += item.RatePerHour;
                }

                if(foremanCost.Count() > 0)
                {
                    cost = int.Parse(quotation.Foreman) * (decimal)hours * (foremanAvRate / foremanCost.Count());
                }
                if (generalworkerCost.Count() > 0)
                {
                    cost += int.Parse(quotation.GeneralWorkers) * (decimal)hours * (generalworkerAvRate / generalworkerCost.Count());
                }
                if (semiskilledworkerCost.Count() > 0)
                {
                    cost += int.Parse(quotation.SemiSkilledWorkers) * (decimal)hours * (semiskilledworkerAvRate / semiskilledworkerCost.Count());
                }
                if (skilledworkerCost.Count() > 0)
                {
                    cost += int.Parse(quotation.SkilledWorkers) * (decimal)hours * (skilledworkerAvRate / skilledworkerCost.Count());
                }
                //

                //Add Mat
                cost += context.QuoteMaterial.Where(m => m.QuotationRequestId == id).Sum(m => m.Quantity);
                //
                //Add Equip
                cost += context.QuoteEquipment.Where(m => m.QuotationRequestId == id).Sum(m => m.Quantity);
                //
            }
            cost = cost + (cost * (decimal)(0.15)); // VAT
            if (DateTime.Parse(quotation.EndDate).Subtract(DateTime.Parse(quotation.StartDate)).TotalDays >= 30)
            {
                cost = cost - (cost * (decimal)(0.05));
            }
            if (quotation.WorkWeekends)
            {
                cost = cost - (cost * (decimal)(0.02));
            }
            return cost;
        }

        public async Task<Quotation> ViewQuote(string reference)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Quotation.Where(q => q.ReferenceNumber == reference && !q.IsDeleted).FirstOrDefaultAsync();
            }
        }

        public async Task AcceptRequestQuote(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Quotation.Find(id);
                context.QuotationRequest.Where(m => m.ReferenceNumber == update.ReferenceNumber
                && !m.IsDeleted).FirstOrDefault().Status = MagicStringReplacer.AcceptedStatus;
                update.Approved = true;
                update.Declined = false;
                await context.SaveChangesAsync();
            }
        }

        public async  Task DeclineRequestQuote(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Quotation.Find(id);
                context.QuotationRequest.Where(m => m.ReferenceNumber == update.ReferenceNumber
                && !m.IsDeleted).FirstOrDefault().Status = MagicStringReplacer.DeclinedStatus;
                update.Approved = false;
                update.Declined = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<QuoteMaterialViewModel>> GetQuoteMaterials(Guid id, string search)
        {
            var mat = new List<QuoteMaterialViewModel>();
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!context.QuoteMaterial.Any(q => q.QuotationRequestId == id)) AddMatToQuote(id);
                foreach (var item in context.QuoteMaterial.Where(q => q.QuotationRequestId == id).ToList())
                {
                    var findMat = context.Material.Find(item.MaterialId);
                    mat.Add(new QuoteMaterialViewModel()
                    {
                        Id = item.Id,
                        MaterialImage = findMat.MaterialImage,
                        MaterialName = findMat.MaterialName,
                        MaterialDescription = findMat.MaterialDescription,
                        Quantity = item.Quantity,
                        IsAdded = item.IsAdded
                    });
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return mat.Where(m => m.MaterialName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                return mat;
            }
         
        }

        private void AddMatToQuote(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var item in context.Material.Where(m => !m.IsDeleted).ToList())
                {
                    context.QuoteMaterial.Add(new QuoteMaterial()
                    {
                        Id = Guid.NewGuid(),
                        QuotationRequestId = id,
                        MaterialId = item.Id,
                        Quantity = 0,
                        IsAdded = false
                    });
                    context.SaveChanges();
                }
            }
        }

        public async Task<List<QuoteEquipmentViewModel>> GetQuoteEquipments(Guid id, string search)
        {
            var equip = new List<QuoteEquipmentViewModel>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!context.QuoteEquipment.Any(q => q.QuotationRequestId == id)) AddEquipToQuote(id);
                foreach (var item in context.QuoteEquipment.Where(q => q.QuotationRequestId == id).ToList())
                {
                    var findEquip = context.Equipment.Find(item.EquipmentId);
                    equip.Add(new QuoteEquipmentViewModel()
                    {
                        Id = item.Id,
                        EquipmentImage = findEquip.EquipmentImage,
                        EquipmentName = findEquip.EquipmentName,
                        Model = findEquip.Model,
                        Brand = findEquip.Brand,
                        Quantity = item.Quantity,
                        IsAdded = item.IsAdded
                    });
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return equip.Where(e => e.EquipmentName.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                return equip;
            }
        }

        private void AddEquipToQuote(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var item in context.Equipment.Where(e => !e.IsDeleted).ToList())
                {
                    context.QuoteEquipment.Add(new QuoteEquipment()
                    {
                        Id = Guid.NewGuid(),
                        QuotationRequestId = id,
                        EquipmentId = item.Id,
                        Quantity = 0,
                        IsAdded = false
                    });
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateMatQty(Guid id, int qty)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = await context.QuoteMaterial.FindAsync(id);
                update.Quantity = qty;
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateEquipQty(Guid id, int qty)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = await context.QuoteEquipment.FindAsync(id);
                update.Quantity = qty;
                await context.SaveChangesAsync();
            }
        }

        enum Type
        {
            foreman,
            generalworker,
            semiskilledworker,
            skilledworker
        }
    }
}