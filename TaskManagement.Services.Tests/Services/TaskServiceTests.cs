namespace TaskManagement.Tests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManagement.BLL.Services;
using TaskManagement.Common.Dto.Task;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.DAL.Extentions;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.Repositories.IRepository;
using TaskManagement.DAL.UoW;
using Xunit;

public class TaskServiceTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly TaskService taskService;

    public TaskServiceTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        taskService = new TaskService(unitOfWorkMock.Object, mapperMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllUserTasksAsync_ShouldReturnFilteredTasks_WhenFiltersApplied()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<DAL.Models.Task>
        {
            new DAL.Models.Task { UserId = userId, Status = Common.Enums.TaskStatus.InProgress, DueDate = DateTime.UtcNow.AddDays(1), Priority = TaskPriority.Medium },
            new DAL.Models.Task { UserId = userId, Status = Common.Enums.TaskStatus.Completed, DueDate = DateTime.UtcNow.AddDays(2), Priority = TaskPriority.High }
        }.AsQueryable();

        var filter = new TaskFilterDto
        {
            Status = Common.Enums.TaskStatus.InProgress,
            DueDateFrom = DateTime.UtcNow,
            SortBy = TaskSortBy.DueDateAsc
        };

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<DAL.Models.Task, bool>>>()))
                .ReturnsAsync(tasks);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act
        var result = await taskService.GetAllUserTasksAsync(userId, filter);

        // Assert
        Assert.Single(result);
        Assert.Equal(Common.Enums.TaskStatus.InProgress, result.First().Status);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var task = new DAL.Models.Task { Id = taskId, UserId = userId };

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<DAL.Models.Task, bool>>>()))
                .ReturnsAsync(task);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act
        var result = await taskService.GetTaskByIdAsync(taskId, userId);

        // Assert
        Assert.Equal(taskId, result.Id);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldThrowKeyNotFoundException_WhenTaskNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<DAL.Models.Task, bool>>>()))
                .ReturnsAsync((DAL.Models.Task)null!);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => taskService.GetTaskByIdAsync(taskId, userId));
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTaskAsync_ShouldReturnTaskId_WhenTaskAddedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskDto = new TaskDto { Title = "New Task" };
        var task = new DAL.Models.Task { Id = Guid.NewGuid(), UserId = userId, Title = "New Task" };

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.Add(It.IsAny<DAL.Models.Task>()));
        mapperMock.Setup(m => m.Map<DAL.Models.Task>(It.IsAny<TaskDto>())).Returns(task);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act
        var result = await taskService.AddTaskAsync(userId, taskDto);

        // Assert
        Assert.Equal(task.Id, result);
        repoMock.Verify(r => r.Add(It.IsAny<DAL.Models.Task>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldReturnTrue_WhenTaskUpdatedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var taskDto = new TaskDto { Title = "Updated Task" };
        var task = new DAL.Models.Task { Id = taskId, UserId = userId, Title = "Old Task" };

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<DAL.Models.Task, bool>>>()))
                .ReturnsAsync(task);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act
        var result = await taskService.UpdateTaskAsync(userId, taskId, taskDto);

        // Assert
        Assert.True(result);
        Assert.Equal("Updated Task", task.Title);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldReturnTrue_WhenTaskDeletedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var task = new DAL.Models.Task { Id = taskId, UserId = userId };

        var repoMock = new Mock<IGenericRepository<DAL.Models.Task>>();
        repoMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<DAL.Models.Task, bool>>>()))
                .ReturnsAsync(task);

        unitOfWorkMock.Setup(uow => uow.GetRepository<DAL.Models.Task>())
                      .Returns(repoMock.Object);

        // Act
        var result = await taskService.DeleteTaskAsync(userId, taskId);

        // Assert
        Assert.True(result);
        repoMock.Verify(r => r.Delete(It.IsAny<DAL.Models.Task>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
}
