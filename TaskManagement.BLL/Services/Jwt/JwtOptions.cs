namespace TaskManagement.BLL.Services.Jwt;

/// <summary>
/// Represents the configuration options for generating JSON Web Tokens (JWTs).
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Gets or sets the secret key used for signing the JWT.
    /// </summary>
    /// <remarks>
    /// This key should be kept secure and should not be hard-coded in the application.
    /// </remarks>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of hours for which the JWT is valid.
    /// </summary>
    /// <remarks>
    /// This value determines the expiration time of the token.
    /// </remarks>
    public int ExpiresHours { get; set; }

    /// <summary>
    /// Gets or sets the issuer of the JWT.
    /// </summary>
    /// <remarks>
    /// The issuer is typically the name of the application or service generating the token.
    /// </remarks>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the audience for which the JWT is intended.
    /// </summary>
    /// <remarks>
    /// The audience is typically the name of the application or service that will be using the token.
    /// </remarks>
    public string Audience { get; set; } = string.Empty;
}
