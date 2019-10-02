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
using Ds3App.Repository.Contract;
using Microsoft.AspNet.Identity;

namespace Ds3App.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly IContractRep _contract;
        public ContractsController():this(new ContractRep())
        {

        }
        public ContractsController(IContractRep contract)
        {
            _contract = contract;
        }
        [Authorize(Roles = "Client")]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            return View(await _contract.GetMyContracts(userId));
        }
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> IndexAdmin()
        {
            return View(await _contract.GetContracts());
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(await _contract.GetContract((Guid)id));
        }
        public async Task<ActionResult> DetailsClient(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(await _contract.GetContract((Guid)id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contract contract)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(contract);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Contract contract)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(contract);
        }
        public async Task<JsonResult> Accept(Guid id)
        {
            await _contract.Accept(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> Decline(Guid id)
        {
            await _contract.Decline(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
    }
}
