using AuthService.Models.DTO;
using AuthService.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private ResponseDTO _response;
    private readonly IAuthService _authService;

    public AuthAPIController(IAuthService authService )
    {
        _authService = authService;
        _response = new();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDTO request)
    {
        var errorMessage = await _authService.Register(request);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            _response.IsSuccess = false;
            _response.Message = errorMessage;
            return BadRequest(_response);
        }
        return Ok(_response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {
        var response = await _authService.Login(request);
        if (response.User is null)
        {
            _response.IsSuccess = false;
            _response.Message = "Username or password is incorrect";
            return BadRequest(_response);
        }
        _response.Result = response;
        return Ok(_response);
    }

    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationDTO request)
    {
        var response = await _authService.AssignRole(request.Email, request.Role.ToUpper());
        if (!response)
        {
            _response.IsSuccess = false;
            _response.Message = "Role was not assigned";
        }
        _response.Result = response;
        return Ok(_response);
    }
}
