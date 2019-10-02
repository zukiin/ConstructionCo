using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ds3App.Repository.Payment;
using Microsoft.AspNet.Identity;
using System.IO;
using Ds3App.Repository.Project;

namespace Ds3App.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentRep _payment;
        private readonly IProjectRep _project;
        public PaymentsController():this(new PaymentRep(), new ProjectRep())
        {

        }
        public PaymentsController(IPaymentRep payment, IProjectRep project)
        {
            _payment = payment;
            _project = project;
        }

        public async Task<ActionResult> CreateProject(Guid id)
        {
            try
            {
                await _project.CreateProject(id);
                return RedirectToAction("IndexAdmin", "Projects");
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                return View(await _payment.GetPaymentByClient(userId));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> IndexAdmin()
        {
            try
            {
                return View(await _payment.GetPaymentsByAdmin());
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> Upload(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                return View(await _payment.GetPayment((Guid)id));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Models.Payment payment, HttpPostedFileBase file)
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
                        string path = Path.Combine(Server.MapPath("~/ProofOfPayment"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        payment.ProofOfPayment = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch
                    {
                        ViewBag.Error = "Document upload was not successfully, please try again.";
                        return View(payment);
                    }
                }
                try
                {
                    await _payment.UploadProofOfPayment(payment);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            return View(payment);
        }
    }
}
