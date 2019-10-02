using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App.Models;

namespace Ds3App.Repository.ProjectTask
{
    public class TaskRep : ITaskRep
    {
        public async Task CreateConstructionTask(ConstructionTask task)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                task.Id = Guid.NewGuid();
                context.ConstructionTask.Add(task);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteConstructionTask(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ConstructionTask.Find(id).IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditConstructionTask(ConstructionTask task)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var update = context.ConstructionTask.Find(task.Id);
                if (update != null)
                {
                    update.TaskName = task.TaskName;
                    update.TaskDescription = task.TaskDescription;
                    update.RatePerHour = task.RatePerHour;
                    update.WorkerTypeId = task.WorkerTypeId;
                    update.IsDeleted = task.IsDeleted;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<TaskViewModel>> GetConstructionTasks()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<TaskViewModel> tasks = new List<TaskViewModel>();
                foreach(var item in await context.ConstructionTask.Where(s => !s.IsDeleted).ToListAsync())
                {
                    tasks.Add(new TaskViewModel()
                    {
                        Id = item.Id,
                        TaskName = item.TaskName,
                        RatePerHour = item.RatePerHour,
                        TaskDescription = item.TaskDescription,
                        WorkerTypeId = context.WorkerType.Find(item.WorkerTypeId).Type,
                        IsDeleted = item.IsDeleted
                    });
                }
                return tasks;
            }
        }

        public async Task<ConstructionTask> GetConstructionTaskToEdit(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.ConstructionTask.FindAsync(id);
            }
        }

        public async Task<IEnumerable<Models.WorkerType>> GetWorkerTypes()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return await context.WorkerType.Where(s => !s.IsDeleted).ToListAsync();
            }
        }
    }
}