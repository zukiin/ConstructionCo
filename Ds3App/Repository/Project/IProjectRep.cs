using Ds3App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.Project
{
    public interface IProjectRep
    {
        Task<IEnumerable<Models.Project>> GetUserProjects(string userId);
        Task<IEnumerable<Models.Project>> GetProjects();
        Task CreateProject(Guid id);
        Task<Models.Project> GetProjectToEdit(Guid id);
        Task EditProject(Models.Project project);
        Task DeleteProject(Guid id);
        Task<IEnumerable<Models.ProjectTask>> GetProjectTasks(Guid id);
        Task AddTaskToProject(Models.ProjectTask task);
        Task<IEnumerable<Models.ConstructionTask>> GetTasks();
        Task DeleteProjectTask(Guid id);
        Task CompleteProjectTask(Guid id);
        Task<Models.ProjectTask> GetProjectTaskToEdit(Guid id);
        Task EditProjectTask(Models.ProjectTask task);
        Task<IEnumerable<Models.ProjectWithForemanViewModel>> GetProjectsWithForeman();
        Task AddForeManToProject(Guid id,string foremanId);
        Task RemoveForeManToProject(Guid id);
        Task<IEnumerable<Models.Project>> GetProjectsForForeman(string userId);
        Task<IEnumerable<WorkerViewModel>> GetWorkers();
        Task<IEnumerable<Models.ProjectTask>> GetWorkerTasks(string userId);
        Task SubmitComment(Guid id,string com);
    }
}
