namespace TaskManagement.BLL.Services;

using TaskManagement.BLL.Services.IService;

internal class TaskService : ITaskService
{
    public Task<int> AddAsync(DAL.Models.Task task)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DAL.Models.Task>> GetAllUserTasks()
    {
        throw new NotImplementedException();
    }

    public Task<DAL.Models.Task> GetTaskById(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int taskId, DAL.Models.Task task)
    {
        throw new NotImplementedException();
    }
}
