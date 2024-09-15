namespace TaskManagement.BLL.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using TaskManagement.BLL.Services.IService;
    using TaskManagement.Common.Dto.Task;
    using TaskManagement.Common.Enums;
    using TaskManagement.Common.Exceptions;
    using TaskManagement.DAL.Models;
    using TaskManagement.DAL.UoW;

    /// <summary>
    /// Provides methods for managing tasks for authenticated users.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance for data access.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get tasks for the authenticated user with filtering and sorting options.
        /// </summary>
        /// <param name="userId">Authenticated user ID.</param>
        /// <param name="filter">Task filter DTO containing optional filtering criteria.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of TaskDto.</returns>
        public async Task<IEnumerable<Task>> GetAllUserTasksAsync(Guid userId, TaskFilterDto filter = null!)
        {
            var taskRepo = this.unitOfWork.GetRepository<Task>();

            var query = await taskRepo.GetAllAsync(t => t.UserId == userId).ConfigureAwait(false);

            if (filter is not null)
            {
                if (filter.Status.HasValue)
                {
                    query = query.Where(t => t.Status == filter.Status.Value);
                }

                if (filter.DueDateFrom.HasValue)
                {
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date >= filter.DueDateFrom.Value.Date);
                }

                if (filter.DueDateTo.HasValue)
                {
                    query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date <= filter.DueDateTo.Value.Date);
                }

                if (filter.Priority.HasValue)
                {
                    query = query.Where(t => t.Priority == filter.Priority.Value);
                }

                switch (filter.SortBy)
                {
                    case TaskSortBy.DueDateAsc:
                        query = query.OrderBy(t => t.DueDate);
                        break;
                    case TaskSortBy.DueDateDesc:
                        query = query.OrderByDescending(t => t.DueDate);
                        break;
                    case TaskSortBy.PriorityAsc:
                        query = query.OrderBy(t => t.Priority);
                        break;
                    case TaskSortBy.PriorityDesc:
                        query = query.OrderByDescending(t => t.Priority);
                        break;
                }
            }

            return query;
        }

        /// <summary>
        /// Retrieves a task by its ID for the authenticated user.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <param name="userId">The authenticated user ID to ensure the task belongs to the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the requested task.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the task is not found or the user does not have access to it.
        /// </exception>
        public async Task<Task> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            var taskRepo = this.unitOfWork.GetRepository<Task>();

            var task = await taskRepo.GetAsync(t => t.Id == taskId && t.UserId == userId).ConfigureAwait(false)
                ?? throw new KeyNotFoundException("Task not found or you do not have access to it.");

            return task;
        }

        /// <summary>
        /// Add a new task for the authenticated user.
        /// </summary>
        /// <param name="userId">Authenticated user ID.</param>
        /// <param name="taskDto">Task creation DTO.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created TaskDto.</returns>
        public async Task<Guid> AddTaskAsync(Guid userId, TaskDto taskDto)
        {
            ArgumentNullException.ThrowIfNull(taskDto);

            var taskRepo = this.unitOfWork.GetRepository<Task>();

            var taskModel = this.mapper.Map<Task>(taskDto);
            taskModel.UserId = userId;

            taskRepo.Add(taskModel);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);

            return taskModel.Id;
        }

        /// <summary>
        /// Update an existing task for the authenticated user.
        /// </summary>
        /// <param name="userId">Authenticated user ID.</param>
        /// <param name="taskId">ID of the task to update.</param>
        /// <param name="taskDto">Task update DTO.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
        public async Task<bool> UpdateTaskAsync(Guid userId, Guid taskId, TaskDto taskDto)
        {
            ArgumentNullException.ThrowIfNull(taskDto);

            var taskRepo = this.unitOfWork.GetRepository<Task>();

            var task = await taskRepo.GetAsync(t => t.Id == taskId && t.UserId == userId).ConfigureAwait(false)
                ?? throw new NotFoundException("Task not found or access denied.");

            task.Title = taskDto.Title ?? task.Title;
            task.Description = taskDto.Description ?? task.Description;
            task.DueDate = taskDto.DueDate ?? task.DueDate;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Delete an existing task for the authenticated user.
        /// </summary>
        /// <param name="userId">Authenticated user ID.</param>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var taskRepo = this.unitOfWork.GetRepository<Task>();

            var task = await taskRepo.GetAsync(t => t.Id == taskId && t.UserId == userId).ConfigureAwait(false)
                ?? throw new NotFoundException("Task not found or access denied.");

            taskRepo.Delete(task);
            await this.unitOfWork.CommitAsync().ConfigureAwait(false);
            return true;
        }
    }
}