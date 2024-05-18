using AuthService.Models;

namespace AuthService.Service.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser appUser, IEnumerable<string> roles);
}
