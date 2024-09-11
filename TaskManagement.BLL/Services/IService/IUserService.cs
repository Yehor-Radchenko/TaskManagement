namespace TaskManagement.BLL.Services.IService;

using System.Threading.Tasks;
using TaskManagement.Common.Dto.User;

/// <summary>
/// Defines the contract for user management services.
/// </summary>
internal interface IUserService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="dto">The DTO containing user registration details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    Task<bool> Register(UserRegistrationDto dto);

    /// <summary>
    /// Changes the password for an existing user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose password needs to be changed.</param>
    /// <param name="dto">The DTO containing the new password details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}
