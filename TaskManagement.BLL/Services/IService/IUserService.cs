namespace TaskManagement.BLL.Services.IService;

using System.Threading.Tasks;
using TaskManagement.Common.Dto.User;
using TaskManagement.DAL.Models;

/// <summary>
/// Defines the contract for user management services.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="dto">The DTO containing user registration details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    Task<bool> RegisterAsync(UserRegistrationDto dto);

    /// <summary>
    /// Changes the password for an existing user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose password needs to be changed.</param>
    /// <param name="dto">The DTO containing the new password details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);

    /// <summary>
    /// Finds a user by their login information.
    /// </summary>
    /// <param name="login">The login identifier (e.g., username or email) of the user to find.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="User"/> object if found, or <c>null</c> if not found.</returns>
    Task<User> FindUserAsync(string login);
}
