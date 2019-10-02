using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ds3App.Repository.Project
{
    public class ProjectRep : IProjectRep
    {
        public async Task AddForeManToProject(Guid id, string foremanId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ForemanAssignedToProject.Add(new ForemanAssignedToProject()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = id,
                    ForemanId = foremanId,
                    IsDeleted = false,
                    DateTimeStamp = DateTime.Now
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task AddTaskToProject(Models.ProjectTask task)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ProjectTask.Add(task);
                await context.SaveChangesAsync();
            }
        }

        public async Task CompleteProjectTask(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var model = context.ProjectTask.Find(id);
                model.IsCompleted = true;
                var project = context.Project.Find(model.ProjectId);
                var quote = context.Quotation.Where(q => q.ReferenceNumber == project.QuotationReference
                && !q.IsDeleted).FirstOrDefault();
                var totalTasks = quote.ForemanTasks + quote.SkilledWorkerTasks + quote.SemiSkilledWorkerTasks + quote.GeneralWorkerTasks;
                int finished = int.Parse(project.CompleteTasks.Substring(0,1)) + 1;
                decimal perc = Math.Round(((decimal)finished / (decimal)totalTasks) * 100, 0);
                project.Progress = $"{perc.ToString()}%";
                if(finished == totalTasks)
                {
                    project.Status = Constant.MagicStringReplacer.CompletedStatus;
                }
                project.CompleteTasks = $"{finished} out of {totalTasks}";

                await context.SaveChangesAsync();
            }
        }

        public async Task CreateProject(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var pay = context.Payment.Find(id);
                var quote = context.Quotation.Where(q => q.ReferenceNumber == pay.QuotationReference
                && !q.IsDeleted).FirstOrDefault();
                var totalTasks = quote.ForemanTasks + quote.SkilledWorkerTasks + quote.SemiSkilledWorkerTasks + quote.GeneralWorkerTasks;
                Models.Project project = new Models.Project();
                project.Id = Guid.NewGuid();
                project.ProjectCost = quote.EstimatedCost;
                project.ProjectName = pay.ProjectName;
                project.ProjectDescription = quote.ProjectDescription;
                project.StartDate = quote.StartDate;
                project.EndDate = quote.EndDate;
                project.Progress = "0%";
                project.Status = Constant.MagicStringReplacer.PendingStatus;
                project.ClientId = quote.ClientId;
                project.CompleteTasks = $"0 out of {totalTasks}";
                project.ForemanTasks = quote.ForemanTasks;
                project.SkilledWorkerTasks = quote.SkilledWorkerTasks;
                project.SemiSkilledWorkerTasks = quote.SemiSkilledWorkerTasks;
                project.GeneralWorkerTasks = quote.GeneralWorkerTasks;
                project.QuotationReference = quote.ReferenceNumber;
                context.Project.Add(project);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteProject(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Project.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteProjectTask(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ProjectTask.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditProject(Models.Project project)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.Project.Find(project.Id);
                if (update != null)
                {
                    update.ProjectName = project.ProjectName;
                    update.ProjectDescription = project.ProjectDescription;
                    update.ProjectCost = project.ProjectCost;
                    update.StartDate = project.StartDate;
                    update.EndDate = project.EndDate;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task EditProjectTask(Models.ProjectTask task)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.ProjectTask.Find(task.Id);
                if (update != null)
                {
                    update.ConstructionTask = task.ConstructionTask;
                    update.IsCompleted = task.IsCompleted;
                    update.IsDeleted = task.IsDeleted;
                    update.ProjectId = task.ProjectId;
                    update.DateAssigned = task.DateAssigned;
                    update.DueDate = task.DueDate;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Models.Project>> GetProjects()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Project.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<IEnumerable<Models.Project>> GetProjectsForForeman(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<Guid> projectIds = new List<Guid>();
                foreach(var item in await context.ForemanAssignedToProject.Where(p => p.ForemanId == userId).ToListAsync())
                {
                    projectIds.Add(item.ProjectId);
                }
                return await context.Project.Where(s => !s.IsDeleted && projectIds.Contains(s.Id)).ToListAsync();
            }
        }

        public async Task<IEnumerable<ProjectWithForemanViewModel>> GetProjectsWithForeman()
        {
            List<ProjectWithForemanViewModel> projectWithForemanViews = new List<ProjectWithForemanViewModel>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach(var item in await context.Project.Where(p => !p.IsDeleted).ToListAsync())
                {
                    var project = context.ForemanAssignedToProject.Where(f => f.ProjectId == item.Id && !f.IsDeleted)?.FirstOrDefault();
                    if (project != null)
                    {
                        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var user = userManager.FindById(project.ForemanId);
                        projectWithForemanViews.Add(new ProjectWithForemanViewModel()
                        {
                            Id = item.Id,
                            QuotationReference = item.QuotationReference,
                            ProjectCost = item.ProjectCost,
                            ProjectDescription = item.ProjectDescription,
                            ProjectName = item.ProjectName,
                            Status = item.Status,
                            Foreman = $"{user.FirstName} {user.LastName}"
                        });
                    }
                    else
                    {
                        projectWithForemanViews.Add(new ProjectWithForemanViewModel()
                        {
                            Id = item.Id,
                            QuotationReference = item.QuotationReference,
                            ProjectCost = item.ProjectCost,
                            ProjectDescription = item.ProjectDescription,
                            ProjectName = item.ProjectName,
                            Status = item.Status,
                            Foreman = $""
                        });
                    }
                }
                return projectWithForemanViews;
            }
        }

        public async Task<IEnumerable<Models.ProjectTask>> GetProjectTasks(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ProjectTask.Where(s => !s.IsDeleted
                && s.ProjectId == id).ToListAsync();
            }
        }

        public async Task<Models.ProjectTask> GetProjectTaskToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ProjectTask.FindAsync(id);
            }
        }

        public async Task<Models.Project> GetProjectToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Project.FindAsync(id);
            }
        }

        public async Task<IEnumerable<ConstructionTask>> GetTasks()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ConstructionTask.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<IEnumerable<Models.Project>> GetUserProjects(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.Project.Where(s => !s.IsDeleted
                && s.ClientId == userId).ToListAsync();
            }
        }

        public async Task<IEnumerable<WorkerViewModel>> GetWorkers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var workers = new List<WorkerViewModel>();
                var models = await context.Users.ToListAsync();
                foreach(var model in models)
                {
                    string role = context.Roles.Find(model.Roles.First().RoleId).Name;
                    if(role == "Worker")
                    {
                        workers.Add(new WorkerViewModel()
                        {
                            Key = model.Id,
                            Value = model.FirstName + " " + model.LastName
                        });
                    }
                }
                return workers;
            }
        }

        public async Task<IEnumerable<Models.ProjectTask>> GetWorkerTasks(string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ProjectTask.Where(p => p.WorkerId == userId && !p.IsDeleted).ToListAsync();
            }
        }

        public async Task RemoveForeManToProject(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ForemanAssignedToProject.Where(p => p.ProjectId == id && !p.IsDeleted).FirstOrDefault().IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task SubmitComment(Guid id, string com)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var project = context.Project.Find(id);
                var client = context.Client.Where(u => u.UserId == project.ClientId).FirstOrDefault();
                context.Feedback.Add(new Models.Feedback()
                {
                    id = Guid.NewGuid(),
                    ClientId = project.ClientId,
                    Project = project.ProjectName,
                    Comment = com,
                    DateTimeStamp = DateTime.Now,
                    Client = $"{client.FirstName} {client.LastName}",
                    ProjectId = id
                });
                await context.SaveChangesAsync();
            }
        }
    }
}