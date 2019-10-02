using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ds3App.Models;
using Ds3App.Repository.Quote;
using Microsoft.AspNet.Identity;
using Ds3App.Extension;
using System.IO;

namespace Ds3App.Controllers
{
    public class QuotationRequestsController : ClientController
    {
        private readonly IQuoteRep _quote;
        public QuotationRequestsController():this(new QuoteRep())
        {

        }
        public QuotationRequestsController(IQuoteRep quote)
        {
            _quote = quote;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            return View(await _quote.GetMyQuotes(userId));
        }
        public ActionResult Create()
        {
            TempData["Ref"] = Helper.GetReference();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QuotationRequest quotationRequest, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".pdf" && ext != ".PDF")
                    {
                        ViewBag.Error = $"Documents should be in a pdf format.";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/ProjectDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        quotationRequest.ProjectDocuments = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Document upload was not successfully, please try again.";
                        return View(quotationRequest);
                    }
                }
                try
                {
                    string userId = User.Identity.GetUserId();
                    quotationRequest.ClientId = userId;
                    await _quote.RequestQuote(quotationRequest);
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
               
                return RedirectToAction("Index");
            }

            return View(quotationRequest);
        }
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(await _quote.GetRequestQuoteToEdit((Guid)id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QuotationRequest quotationRequest, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".pdf" && ext != ".PDF")
                    {
                        ViewBag.Error = $"Documents should be in a pdf format.";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/ProjectDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        quotationRequest.ProjectDocuments = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Document upload was not successfully, please try again.";
                        return View(quotationRequest);
                    }
                }
                try
                {
                    await _quote.EditRequestQuote(quotationRequest);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            return View(quotationRequest);
        }
        public async Task<JsonResult> Delete(Guid id)
        {
            await _quote.DeleteRequestQuote(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ViewQuote(string reference)
        {
            try
            {
                Quotation quote = new Quotation();
                quote = await _quote.ViewQuote(reference);

                /** VAT and Discounts */
                decimal estimateCost = quote.EstimatedCost;
                decimal discount = 0;
                decimal subTotal = 0;

                decimal initialCost = quote.EstimatedCost - (quote.EstimatedCost * (decimal)(0.15));
                double durationInDays = quote.EndDate.Subtract((quote.StartDate)).TotalDays;

                if (quote.WorkWeekends)
                {
                    initialCost = initialCost + (initialCost * (decimal)(0.02));
                }
                if (durationInDays >= 30)
                {
                    initialCost = initialCost + (initialCost * (decimal)(0.05));
                }
                TempData["Total"] = initialCost.ToString("C");

                decimal VAT = initialCost * (decimal)(0.15);

                initialCost = initialCost + VAT;

                if (quote.WorkWeekends)
                {
                    discount = initialCost * (decimal)(0.02);
                }
                if (durationInDays >= 30)
                {
                    discount = discount + (initialCost * (decimal)(0.05));
                }

                subTotal = initialCost; /** Weird, but the grand total comes preCalculated*/
                TempData["Discount"] = discount.ToString("C");
                TempData["VAT"] = VAT.ToString("C");
                TempData["SubTotal"] = subTotal.ToString("C");
                return View(await _quote.ViewQuote(reference));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<JsonResult> Decline(Guid id)
        {
            await _quote.DeclineRequestQuote(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> Accept(Guid id)
        {
            await _quote.AcceptRequestQuote(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
    }
}
