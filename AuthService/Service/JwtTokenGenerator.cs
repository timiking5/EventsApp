using AuthService.Models;
using AuthService.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Service;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;
    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    public string GenerateToken(ApplicationUser appUser, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.Secret);

        var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, appUser.UserName)
            };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _options.Audience,
            Issuer = _options.Issuer,
            Subject = new(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
