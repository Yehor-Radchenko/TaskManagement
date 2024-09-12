namespace TaskManagement.BLL.Services.Jwt;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.BLL.Services.IService;
using TaskManagement.DAL.Models;

internal class JwtService : IJwtService
{
    private readonly JwtOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="jwtOptions">Required set of options for generating a token.</param>
    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        this.options = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(nameof(user));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Email, user.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey));
        var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(this.options.ExpiresHours)),
            signingCredentials: sign);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
