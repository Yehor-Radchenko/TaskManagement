namespace TaskManagement.BLL.Services.Jwt;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.BLL.Services.IService;
using TaskManagement.DAL.Models;

/// <summary>
/// Provides methods for generating JSON Web Tokens (JWTs) for user authentication.
/// </summary>
internal class JwtService : IJwtService
{
    private readonly JwtOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="jwtOptions">Options required for generating a token, including secret key, issuer, audience, and expiration settings.</param>
    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        this.options = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> parameter is <c>null</c>.</exception>
    public string GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey));
        var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: this.options.Issuer,
            audience: this.options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(this.options.ExpiresHours),
            signingCredentials: sign);

        Log.Information("User {Email} authenticated successfully", user.Email);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
