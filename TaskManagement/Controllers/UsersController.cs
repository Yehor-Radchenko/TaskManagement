namespace TaskManagement.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagement.BLL.Services;
using TaskManagement.BLL.Services.IService;
using TaskManagement.Common.Dto.User;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IJwtService jwtService;
    private readonly IUserService userService;

    public UsersController(
        IHttpContextAccessor httpContextAccessor,
        IJwtService jwtService,
        IUserService userService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.jwtService = jwtService;
        this.userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] SignInDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new { Message = "Invalid login data." });
        }

        var user = await this.userService.FindUserAsync(dto.Login).ConfigureAwait(false);

        if (PasswordHasherService.VerifyPassword(dto.Password, user.PasswordHash))
        {
            var token = this.jwtService.GenerateToken(user);

            this.httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt-token", token);

            return this.Ok(token);
        }
        else
        {
            Log.Information("Failed login attempt to {Login} account", dto.Login);

            return this.Unauthorized($"Failed login attempt to {dto.Login} account");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] UserRegistrationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest("Invalid login data.");
        }

        if (await this.userService.RegisterAsync(dto).ConfigureAwait(false))
        {
            return this.Created();
        }

        return this.BadRequest();
    }

    [HttpPost("change-password/{userId:guid}")]
    public async Task<IActionResult> ChangePasswordAsync(Guid userId, [FromBody] ChangePasswordDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var success = await this.userService.ChangePasswordAsync(userId, dto).ConfigureAwait(false);

        if (success)
        {
            return this.Ok("Password changed successfully.");
        }

        return this.BadRequest("Password change failed.");
    }
}