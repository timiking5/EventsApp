using AuthService.Data;
using AuthService.Models.DTO;
using AuthService.Service;
using AuthService.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private ResponseDTO _response;
    private readonly IAuthService _authService;
    private readonly MessageSender _messageSender;
    private readonly AppDbContext _db;

    public AuthAPIController(IAuthService authService, MessageSender messageSender, AppDbContext db)
    {
        _authService = authService;
        _response = new();
        _messageSender = messageSender;
        _db = db;
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
        _messageSender.SendUserMessage(new UserDTO {
            Id = _db.Users.FirstOrDefault(x => x.Email == request.Email)!.Id,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        });
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
