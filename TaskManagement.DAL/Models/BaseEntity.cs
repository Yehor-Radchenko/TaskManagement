namespace TaskManagement.DAL.Models;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
