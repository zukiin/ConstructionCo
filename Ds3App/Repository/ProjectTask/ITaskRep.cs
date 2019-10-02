using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App.Repository.ProjectTask
{
    public interface ITaskRep
    {
        Task<IEnumerable<Models.TaskViewModel>> GetConstructionTasks();
        Task CreateConstructionTask(Models.ConstructionTask supplier);
        Task<Models.ConstructionTask> GetConstructionTaskToEdit(Guid id);
        Task EditConstructionTask(Models.ConstructionTask supplier);
        Task DeleteConstructionTask(Guid id);
        Task<IEnumerable<Models.WorkerType>> GetWorkerTypes();
    }
}
