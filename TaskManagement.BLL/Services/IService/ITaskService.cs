namespace TaskManagement.BLL.Services.IService;

using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Common.Dto.Task;

public interface ITaskService
{
    Task<IEnumerable<DAL.Models.Task>> GetAllUserTasksAsync(Guid userId, TaskFilterDto filter = null!);

    Task<DAL.Models.Task> GetTaskByIdAsync(Guid taskId, Guid userId);

    Task<Guid> AddTaskAsync(Guid userId, TaskDto taskDto);

    Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);

    Task<bool> UpdateTaskAsync(Guid userId, Guid taskId, TaskDto taskDto);
}
