using Ds3App.Models;
using Ds3App.Repository.Contract;
using Ds3App.Repository.Project;
using Ds3App.Repository.Quote;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace Ds3App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IQuoteRep _quote;
        private readonly IContractRep _contract;
        private readonly IProjectRep _project;
        public AdminController() : this(new QuoteRep(), new ContractRep(), new ProjectRep())
        {

        }
        public AdminController(IQuoteRep quote, IContractRep contract, IProjectRep project)
        {
            _quote = quote;
            _contract = contract;
            _project = project;
        }
        public async Task<JsonResult> RemoveForemanToProjectAjax(Guid id)
        {
            await _project.RemoveForeManToProject(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> AddForemanToProjectAjax(string array)
        {
            var newstring = array.Split('|').ToArray();
            await _project.AddForeManToProject(Guid.Parse(newstring[1]), newstring[0]);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> AddForemanToProject(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    foreach (var item in await context.Users.Where(u => !u.IsDeleted).ToListAsync())
                    {
                        if (userManager.IsInRole(item.Id, "Foreman"))
                        {
                            applicationUsers.Add(new ApplicationUser()
                            {
                                Id = item.Id,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                Contact = item.Contact,
                                Email = item.Email
                            });
                        }
                    }
                    TempData["ProjectId"] = id;
                    return View(applicationUsers);
                }
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> ProjectWithForeman()
        {
            try
            {
                return View(await _project.GetProjectsWithForeman());
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> Index(string search = null)
        {
            try
            {
                return View(await _quote.GetQuotes(search));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        [HttpGet]
        public async Task<ActionResult> CreateQuote(Guid? id, string search = null)
        {
            try
            {
                var model = await _quote.GetRequestQuoteToEdit((Guid)id);
                QuotationViewModel quotation = new QuotationViewModel();
                quotation.ReferenceNumber = model.ReferenceNumber;
                quotation.ProjectName = model.ProjectName;
                quotation.ProjectDescription = model.ProjectDescription;
                quotation.ClientId = model.ClientId;
                quotation.Documents = model.ProjectDocuments;
                quotation.QuoteRequestId = model.Id;

                quotation.Foreman = "1";
                //quotation.GeneralWorkers = "1";

                quotation.QuoteMaterials = await _quote.GetQuoteMaterials((Guid)id, search);
                quotation.QuoteEquipments = await _quote.GetQuoteEquipments((Guid)id, search);
                // if you want  eg 5 items loaded initial (ToPagedList, 5)
                quotation.QuoteMaterials = quotation.QuoteMaterials.ToPagedList(1, 2);
                quotation.QuoteEquipments = quotation.QuoteEquipments.ToPagedList(1, 2);
                return View(quotation);
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateQuote(QuotationViewModel quotation)
        {
            if (ModelState.IsValid)
            {
                if(DateTime.Parse(quotation.EndDate)<DateTime.Parse(quotation.StartDate))
                {
                    ViewBag.Error = "End date can not be less then start date";
                }
                try
                {
                    quotation.Foreman = "1";
                    await _quote.CreateQuote(quotation);
                    TempData["Success"] = $"Quote created successfully and an email sent to the client.";
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            return View(quotation);
        }

        public async Task<ActionResult> CreateContract(Guid? id)
        {
            try
            {
                Guid contractId = await _contract.CreateContract((Guid)id);
                return RedirectToAction("Details", "Contracts", new { id = contractId });
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateMatQty(string array)
        {
            string[] val = array.Split(',');
            Guid id = Guid.Parse(val[0]);
            int qty = int.Parse(val[1]);
            await _quote.UpdateMatQty(id, qty);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEquipQty(string array)
        {
            string[] val = array.Split(',');
            Guid id = Guid.Parse(val[0]);
            int qty = int.Parse(val[1]);
            await _quote.UpdateEquipQty(id, qty);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<PartialViewResult> SearchEquiptment(Guid id, string searchTerm)
        {
            if (searchTerm == "undefined" || searchTerm == "")
            {
                searchTerm = null;
            }
            QuotationViewModel quotation = new QuotationViewModel();
            quotation.QuoteEquipments = await _quote.GetQuoteEquipments(id, searchTerm);
            return PartialView("_EquiptmentPartial", quotation);
        }

        [HttpGet]
        public async Task<PartialViewResult> PagedEquiptment(Guid id, int? page)
        {
            QuotationViewModel quotation = new QuotationViewModel();
            quotation.QuoteEquipments = await _quote.GetQuoteEquipments(id, null);
            quotation.QuoteEquipments = quotation.QuoteEquipments.ToPagedList(page ?? 1, 2);
            return PartialView("_LoadMoreEquiptment", quotation);
        }

        [HttpGet]
        public async Task<PartialViewResult> SearchMarterial(Guid id, string searchTerm)
        {
            if (searchTerm == "undefined" || searchTerm == "")
            {
                searchTerm = null;
            }
            QuotationViewModel quotation = new QuotationViewModel();
            quotation.QuoteMaterials = await _quote.GetQuoteMaterials(id, searchTerm);
            return PartialView("_MaterialPartial", quotation);
        }

        [HttpGet]
        public async Task<PartialViewResult> PagedMarterial(Guid id, int? page)
        {
            QuotationViewModel quotation = new QuotationViewModel();
            quotation.QuoteMaterials = await _quote.GetQuoteMaterials(id, null);
            quotation.QuoteMaterials = quotation.QuoteMaterials.ToPagedList(page ?? 1, 2);
            return PartialView("_LoadMoreMaterial", quotation);
        }
    }
}