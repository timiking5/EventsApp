using AuthService.Models.DTO;

namespace AuthService.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationDTO reg);
    Task<LoginResponseDTO> Login(LoginDTO login);
    Task<bool> AssignRole(string email, string roleName);
}
