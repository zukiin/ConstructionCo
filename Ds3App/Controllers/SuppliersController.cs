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
using Ds3App.Repository.Material;
using Ds3App.Repository.Supplier;

namespace Ds3App.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierRep _supplier;
        public SuppliersController():this(new SupplierRep())
        {

        }
        public SuppliersController(ISupplierRep supplier)
        {
            _supplier = supplier;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _supplier.GetSuppliers());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplier.CreateSupplier(supplier);
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(await _supplier.GetSupplierToEdit((Guid)id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplier.EditSupplier(supplier);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }


        public async Task<JsonResult> Delete(Guid id)
        {
            await _supplier.DeleteSupplier(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
    }
}
