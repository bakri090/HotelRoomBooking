using Booking.Application.DTOs.Auth;
using Booking.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Booking.Infrastructure.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public  (string token, int expiresIn) GenerateToken(GenerateTokenRequest
      user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
						new(JwtRegisteredClaimNames.GivenName, user.FirstName),
						new(JwtRegisteredClaimNames.FamilyName, user.LastName),
				};
		foreach (var role in roles)
		{
			claims.Add(new Claim("role", role));
		}

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: signingCredentials);

        return (token: new JwtSecurityTokenHandler().WriteToken(token),expiresIn: _options.ExpirationMinutes * 60);
    }


}