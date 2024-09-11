namespace TaskManagement.DAL.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskManagement.Common.Enums;

public class Task : BaseEntity
{
    public Task()
    {
        this.Status = TaskStatus.Pending;
        this.Priority = TaskPriority.Medium;
    }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Required]
    public TaskStatus Status { get; set; }

    [Required]
    public TaskPriority Priority { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;
}