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
using Ds3App.Repository.ProjectTask;

namespace Ds3App.Controllers
{
    public class ConstructionTasksController : AdminController
    {
        private readonly ITaskRep _task;
        public ConstructionTasksController() : this(new TaskRep())
        {

        }
        public ConstructionTasksController(ITaskRep task)
        {
            _task = task;
        }
        public async Task<ActionResult> IndexAdminTask()
        {
            try
            {
                return View(await _task.GetConstructionTasks());
            }
            catch
            {
                return RedirectToAction("BadRequest","Home");
            }
            
        }
        public async Task<ActionResult> Create()
        {
            ViewBag.Worker = new SelectList(await _task.GetWorkerTypes(), "Id", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ConstructionTask constructionTask)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _task.CreateConstructionTask(constructionTask);
                    return RedirectToAction("IndexAdminTask");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            if(constructionTask.WorkerTypeId == null)
            {
                ViewBag.Worker = new SelectList(await _task.GetWorkerTypes(), "Id", "Type");
            }
            else
            {
                ViewBag.Worker = new SelectList(await _task.GetWorkerTypes(), "Id", "Type", constructionTask.WorkerTypeId);
            }
          
            return View(constructionTask);
        }
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var model = await _task.GetConstructionTaskToEdit((Guid)id);
                ViewBag.Worker = new SelectList(await _task.GetWorkerTypes(), "Id", "Type", model.WorkerTypeId);
                return View(model);
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ConstructionTask constructionTask)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _task.EditConstructionTask(constructionTask);
                    return RedirectToAction("IndexAdminTask");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            ViewBag.Worker = new SelectList(await _task.GetWorkerTypes(), "Id", "Type", constructionTask.WorkerTypeId);
            return View(constructionTask);
        }

        public async Task<JsonResult> Delete(Guid id)
        {
            await _task.DeleteConstructionTask(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }
    }
}
