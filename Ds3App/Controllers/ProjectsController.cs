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
using Ds3App.Repository.Project;
using Microsoft.AspNet.Identity;

namespace Ds3App.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRep _project;
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ProjectsController() : this(new ProjectRep())
        {

        }
        public ProjectsController(IProjectRep project)
        {
            _project = project;
        }

        public async Task<ActionResult> Create(Guid id)
        {
            TempData["ProjectId"] = id;
            ViewBag.ConTask = new SelectList(await _project.GetTasks(), "Id", "TaskName");
            ViewBag.Workers = new SelectList(await _project.GetWorkers(), "Key", "Value");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.ProjectTask task)
        {
            task.Id = Guid.NewGuid();
            task.DateAssigned = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    await _project.AddTaskToProject(task);
                    return RedirectToAction("IndexForeman");
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            if (task.ConstructionTask == Guid.Empty)
            {
                ViewBag.ConTask = new SelectList(await _project.GetTasks(), "Id", "TaskName");
            }
            else
            {
                ViewBag.ConTask = new SelectList(await _project.GetTasks(), "Id", "TaskName", task.ConstructionTask);
            }

            if (string.IsNullOrEmpty(task.WorkerId))
            {
                ViewBag.Workers = new SelectList(await _project.GetWorkers(), "Key", "Value");

            }
            else
            {
                ViewBag.Workers = new SelectList(await _project.GetWorkers(), "Key", "Value", task.WorkerId);

            }
            return View(task);
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                return View(await _project.GetUserProjects(userId));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> IndexForeman()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                return View(await _project.GetProjectsForForeman(userId));
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
                return View(await _project.GetProjects());
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> ViewTasks(Guid id)
        {
            try
            {
                TempData["ProjectId"] = id;
                return View(await _project.GetProjectTasks(id));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }

        public async Task<ActionResult> MyTasks()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                return View(await _project.GetWorkerTasks(userId));
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var model = await _project.GetProjectTaskToEdit((Guid)id);
                ViewBag.ConTask = new SelectList(await _project.GetTasks(), "Id", "TaskName", model.Id);
                return View(model);
            }
            catch
            {
                return RedirectToAction("BadRequest", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _project.EditProjectTask(task);
                    return RedirectToAction("ViewTasks", new { id = task.ProjectId });
                }
                catch
                {
                    return RedirectToAction("BadRequest", "Home");
                }
            }
            ViewBag.ConTask = new SelectList(await _project.GetTasks(), "Id", "TaskName", task.Id);
            return View(task);
        }

        public async Task<JsonResult> DeleteTask(Guid id)
        {
            await _project.DeleteProjectTask(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> CompleteTask(Guid id)
        {
            await _project.CompleteProjectTask(id);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> Comment(Com model)
        {
            await _project.SubmitComment(Guid.Parse(model.id), model.com);
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProjectReports()
        {
            Project p = new Project();
            return View(db.Project.ToList());
        }

        [HttpGet]
        public ActionResult ProjectReport(Guid id)
        {
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult ProjectReport(Guid id, Project project, string extra)
        {
            Project p = new Project();
            p = db.Project.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                p.ProjectReport = project.ProjectReport;
                db.SaveChanges();
                return RedirectToAction("ProjectReports");
            }
            return View();
        }
    }

    public class Com
    {
        public string id { get; set; }
        public string com { get; set; }
    }
}
