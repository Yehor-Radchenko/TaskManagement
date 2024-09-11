namespace TaskManagement.BLL.Services.IService;

using System.Collections.Generic;
using System.Threading.Tasks;

internal interface ITaskService
{
    Task<IEnumerable<DAL.Models.Task>> GetAllUserTasks();

    Task<DAL.Models.Task> GetTaskById(int taskId);

    Task<int> AddAsync(DAL.Models.Task task);

    Task<bool> DeleteAsync(int taskId);

    Task<bool> UpdateAsync(int taskId, DAL.Models.Task task);
}
