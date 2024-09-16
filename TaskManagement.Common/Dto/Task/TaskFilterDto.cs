namespace TaskManagement.Common.Dto.Task;

using System;
using TaskManagement.Common.Enums;

/// <summary>
/// Represents the filter criteria for retrieving tasks.
/// </summary>
public class TaskFilterDto
{
    /// <summary>
    /// Gets or sets the task status to filter by.
    /// </summary>
    public TaskStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the start date for filtering tasks by due date.
    /// </summary>
    public DateTime? DueDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the end date for filtering tasks by due date.
    /// </summary>
    public DateTime? DueDateTo { get; set; }

    /// <summary>
    /// Gets or sets the priority to filter by.
    /// </summary>
    public TaskPriority? Priority { get; set; }

    /// <summary>
    /// Gets or sets the sorting option for the task list.
    /// </summary>
    public TaskSortBy SortBy { get; set; } = TaskSortBy.None;
}
