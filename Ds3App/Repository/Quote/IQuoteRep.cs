using Ds3App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Quote
{
    public interface IQuoteRep
    {
        Task RequestQuote(QuotationRequest request);
        Task<IEnumerable<QuotationRequest>> GetMyQuotes(string userId);
        Task<IEnumerable<QuotationRequest>> GetQuotes(string search);
        Task DeleteRequestQuote(Guid id);
        Task<QuotationRequest> GetRequestQuoteToEdit(Guid id);
        Task EditRequestQuote(QuotationRequest request);
        Task CreateQuote(QuotationViewModel quotation);
        Task<Quotation> ViewQuote(string reference);
        Task AcceptRequestQuote(Guid id);
        Task DeclineRequestQuote(Guid id);
        Task<List<QuoteMaterialViewModel>> GetQuoteMaterials(Guid id, string search);
        Task<List<QuoteEquipmentViewModel>> GetQuoteEquipments(Guid id, string search);
        Task UpdateMatQty(Guid id, int qty);
        Task UpdateEquipQty(Guid id, int qty);
    }
}
