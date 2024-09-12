namespace TaskManagement.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskManagement.BLL.Services;
using TaskManagement.BLL.Services.IService;
using TaskManagement.Common.Dto.User;
using TaskManagement.Common.ResponseModels;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IJwtService jwtService;
    private readonly ILogger<UsersController> logger;
    private readonly IUserService userService;

    public UsersController(
        IHttpContextAccessor httpContextAccessor,
        IJwtService jwtService,
        ILogger<UsersController> logger,
        IUserService userService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.jwtService = jwtService;
        this.logger = logger;
        this.userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(SignInDto dto)
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
            this.logger.LogInformation("User {Login} authenticated successfully", dto.Login);

            this.httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt-token", token);

            return this.Ok(new ApiResponse<string> { Data = token });
        }
        else
        {
            this.logger.LogError("Failed login attempt to {Login} account", dto.Login);

            return this.Unauthorized();
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserRegistrationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(new { Message = "Invalid login data." });
        }

        if (await this.userService.RegisterAsync(dto).ConfigureAwait(false))
        {
            return this.Created();
        }

        return this.BadRequest();
    }
}