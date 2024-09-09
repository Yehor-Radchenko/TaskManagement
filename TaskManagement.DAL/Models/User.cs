namespace TaskManagement.DAL.Models;

using System.ComponentModel.DataAnnotations;

public class User : BaseEntity
{
    [Required]
    [StringLength(100)]
    [RegularExpression("^(?i)(((?=.{6,21}$)[a-z\\d]+\\.[a-z\\d]+)|[a-z\\d]{5,20})$\r\n")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Task> Tasks { get; } = new List<Task>();
}