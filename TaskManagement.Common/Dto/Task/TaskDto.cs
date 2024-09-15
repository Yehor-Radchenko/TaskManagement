namespace TaskManagement.Common.Dto.Task;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Dto.Validation;

public class TaskDto
{
    [Required(ErrorMessage = "Task Title is required.")]
    [MaxLength(50, ErrorMessage = "Title should be less than 50 symbols long.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "Description should be less than 200 symbols long.")]
    public string Description { get; set; } = string.Empty;

    [FutureDate]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "Task Status is required.")]
    public TaskStatus Status { get; set; }

    [Required(ErrorMessage = "Task Priority is required.")]
    public TaskPriority Priority { get; set; }
}