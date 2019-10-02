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
using Ds3App.Repository.Feedback;

namespace Ds3App.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackRep _feedback;
        public FeedbacksController() : this(new FeedbackRep())
        {

        }
        public FeedbacksController(IFeedbackRep feedback)
        {
            _feedback = feedback;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                return View(await _feedback.GetFeedback());
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _feedback.CreateFeedback(feedback);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            return View(feedback);
        }
        public async Task<JsonResult> Delete(Guid id)
        {
            await _feedback.DeleteFeedback(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
