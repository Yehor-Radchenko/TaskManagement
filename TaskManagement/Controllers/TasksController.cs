using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BLL.Services.IService;
using TaskManagement.Common.Dto.Task;

namespace TaskManagement.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.GetAuthenticatedUserId();
            var createdTaskId = await this.taskService.AddTaskAsync(userId, taskDto).ConfigureAwait(false);
            return this.CreatedAtAction(nameof(this.GetTaskById), new { id = createdTaskId }, taskDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDto filter)
        {
            var userId = this.GetAuthenticatedUserId();
            var tasks = await this.taskService.GetAllUserTasksAsync(userId, filter).ConfigureAwait(false);
            return this.Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = this.GetAuthenticatedUserId();
            var task = await this.taskService.GetTaskByIdAsync(id, userId).ConfigureAwait(false);
            return this.Ok(task);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDto taskDto)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.GetAuthenticatedUserId();
            var success = await this.taskService.UpdateTaskAsync(userId, id, taskDto).ConfigureAwait(false);
            if (success)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound("Task not found or access denied.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = this.GetAuthenticatedUserId();
            var success = await this.taskService.DeleteTaskAsync(userId, id).ConfigureAwait(false);
            if (success)
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound("Task not found or access denied.");
            }
        }

        private Guid GetAuthenticatedUserId()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            return Guid.Parse(userId);
        }
    }
}
