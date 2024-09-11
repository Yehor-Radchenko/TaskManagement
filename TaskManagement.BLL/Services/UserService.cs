namespace TaskManagement.BLL.Services;

using AutoMapper;
using TaskManagement.BLL.Services.IService;
using TaskManagement.Common.Dto.User;
using TaskManagement.Common.Exceptions;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.UoW;

/// <summary>
/// Provides services related to user management such as registration and password change.
/// </summary>
internal class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for interacting with repositories.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between objects.</param>
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="dto">The DTO containing user registration details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="dto"/> is null.</exception>
    /// <exception cref="ConflictException">Thrown when a user with the same email or username already exists.</exception>
    public async Task<bool> Register(UserRegistrationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var userRepo = this.unitOfWork.GetRepository<User>();

        if (await userRepo.ExistsAsync(u => (u.Email == dto.Email || u.Username == dto.Username)).ConfigureAwait(false))
        {
            throw new ConflictException($"User with such email or username already exists.");
        }

        var userModel = this.mapper.Map<User>(dto);
        userModel.CreatedAt = DateTime.UtcNow;
        userModel.UpdatedAt = DateTime.UtcNow;

        userRepo.Add(userModel);
        await this.unitOfWork.CommitAsync().ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Changes the password for an existing user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose password needs to be changed.</param>
    /// <param name="dto">The DTO containing the new password details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating success.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="dto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no user is found with the specified <paramref name="userId"/>.</exception>
    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var userRepo = this.unitOfWork.GetRepository<User>();

        var existingUserModel = await userRepo.GetAsync(u => u.Id == userId).ConfigureAwait(false)
            ?? throw new KeyNotFoundException("User with specified id had not been found.");

        existingUserModel.PasswordHash = PasswordHasherService.HashPassword(dto.Password);
        existingUserModel.UpdatedAt = DateTime.UtcNow;

        await this.unitOfWork.CommitAsync().ConfigureAwait(false);
        return true;
    }
}