namespace TaskManagement.Common.Dto.User;

using System.ComponentModel.DataAnnotations;

public class SignInDto
{
    [Required(ErrorMessage = "Username or email is required.")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
