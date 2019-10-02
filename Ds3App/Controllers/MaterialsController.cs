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
using System.IO;
using Ds3App.Extension;

namespace Ds3App.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialRep _material;
        private readonly ISupplierRep _supplier;
        public MaterialsController() : this(new MaterialRep(), new SupplierRep())
        {

        }
        public MaterialsController(IMaterialRep material, ISupplierRep supplier)
        {
            _material = material;
            _supplier = supplier;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _material.GetMaterials());
        }
        public async Task<ActionResult> Create()
        {
            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Material material, HttpPostedFileBase file)
        {
      
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (!Helper.IsImage(ext))
                    {
                        if (material.Supplier == Guid.Empty)
                        {
                            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName");
                        }
                        else
                        {
                            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName", material.Supplier);
                        }
                        ViewBag.Error = $"Invalid image format.";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/ProjImages"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        material.MaterialImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        if (material.Supplier == Guid.Empty)
                        {
                            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName");
                        }
                        else
                        {
                            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName", material.Supplier);
                        }
                        ViewBag.Error = "Image upload was not successfully, please try again.";
                        return View(material);
                    }
                }
             
                await _material.CreateMaterial(material);
                return RedirectToAction("Index");
            }
            if (material.Supplier == Guid.Empty)
            {
                ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName");
            }
            else
            {
                ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName", material.Supplier);
            }
            return View(material);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await _material.GetMaterialToEdit((Guid)id);
            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName", model.Supplier);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Material material, HttpPostedFileBase file)
        {
            ViewBag.DDLSupplier = new SelectList(await _supplier.GetSuppliers(), "Id", "SupplierName", material.Supplier);

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (!Helper.IsImage(ext))
                    {
                        ViewBag.Error = $"Invalid image format.";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/ProjImages"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        material.MaterialImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Image upload was not successfully, please try again.";
                        return View(material);
                    }
                }
                await _material.EditMaterial(material);
                return RedirectToAction("Index");
            }
            return View(material);
        }
        public async Task<JsonResult> Delete(Guid id)
        {
            await _material.DeleteMaterial(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
    }
}
