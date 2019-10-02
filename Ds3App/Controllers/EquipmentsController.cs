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
using Ds3App.Repository.Equipment;
using System.IO;
using Ds3App.Extension;

namespace Ds3App.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly IEquipmentRep _equipment;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public EquipmentsController() : this(new EquipmentRep())
        {

        }
        public EquipmentsController(IEquipmentRep equipment)
        {
            _equipment = equipment;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _equipment.GetEquipments());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Equipment equipment, HttpPostedFileBase file)
        {
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
                        equipment.EquipmentImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Image upload was not successfully, please try again.";
                        return View(equipment);
                    }
                }
                await _equipment.CreateEquipment(equipment);
                return RedirectToAction("Index");
            }

            return View(equipment);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(await _equipment.GetEquipmentToEdit((Guid)id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Equipment equipment, HttpPostedFileBase file)
        {
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
                        equipment.EquipmentImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Image upload was not successfully, please try again.";
                        return View(equipment);
                    }
                }
                await _equipment.EditEquipment(equipment);
                return RedirectToAction("Index");
            }
            return View(equipment);
        }

        public async Task<JsonResult> Delete(Guid id)
        {
            await _equipment.DeleteEquipment(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BookingConfirmation(Guid id)
        {
            Equipment equipment = new Equipment();
            equipment = db.Equipment.FirstOrDefault(e => e.Id == id);
            return View(equipment);
        }

        [HttpPost]
        public ActionResult BookingConfirmation(Guid id, Equipment equipment)
        {
            equipment = db.Equipment.FirstOrDefault(e => e.Id == id);
            if (equipment != null)
            {
                equipment.LastMaintenance = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Failed to confirm booking for maintenance";
            return View(equipment);
        }
    }
}
