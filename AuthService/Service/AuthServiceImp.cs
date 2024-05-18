using AuthService.Data;
using AuthService.Models;
using AuthService.Models.DTO;
using AuthService.Service.IService;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AuthService.Service;

public class AuthServiceImp : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public AuthServiceImp(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        if (user is null)
        {
            return false;
        }
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        await _userManager.AddToRoleAsync(user, roleName);
        return true;
    }

    public async Task<LoginResponseDTO> Login(LoginDTO login)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == login.Email.ToLower());
        // await TestMe(user, login.Password);
        bool isValid = await _userManager.CheckPasswordAsync(user, login.Password);
        if (user is null || !isValid)
        {
            return new LoginResponseDTO()
            {
                User = null,
                Token = ""
            };
        }
        var roles = await _userManager.GetRolesAsync(user);
        UserDTO userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        };

        LoginResponseDTO response = new()
        {
            User = userDto,
            Token = _jwtTokenGenerator.GenerateToken(user, roles)
        };
        return response;
    }

    public async Task<string> Register(RegistrationDTO reg)
    {
        ApplicationUser user = new()
        {
            UserName = reg.Email,
            Email = reg.Email,
            NormalizedEmail = reg.Email.ToUpper(),
            FirstName = reg.FirstName,
            LastName = reg.LastName,
            PhoneNumber = reg.PhoneNumber
        };
        try
        {
            var result = await _userManager.CreateAsync(user, reg.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.ApplicationUsers.First(x => x.UserName == user.UserName);
                UserDTO userDto = new()
                {
                    Email = userToReturn.Email,
                    Id = userToReturn.Id,
                    FirstName = userToReturn.FirstName,
                    LastName = userToReturn.LastName,
                    PhoneNumber = userToReturn.PhoneNumber
                };
                return "";
            }
            return result.Errors.FirstOrDefault().Description;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //public async Task<bool> TestMe(ApplicationUser user, string password)
    //{
    //    var hasher = _userManager.PasswordHasher;
    //    string passwordhash1 = hasher.HashPassword(user, "Coding@1234?");
    //    string passwordhash2 = hasher.HashPassword(user, "Coding@1234?");

    //    bool equal = passwordhash1 == passwordhash2;

    //    PasswordVerificationResult password_is_correct_1 = hasher.VerifyHashedPassword(user, passwordhash1, "Coding@1234?");
    //    PasswordVerificationResult password_is_correct_2 = hasher.VerifyHashedPassword(user, passwordhash2, "Coding@1234?");

    //    var valid = await _userManager.CheckPasswordAsync(user, password);

    //    ApplicationUser user2 = new()
    //    {
    //        UserName = "timiking@gmail.com",
    //        Email = "timiking@gmail.com",
    //        NormalizedEmail = "timiking@gmail.com".ToUpper(),
    //        FirstName = "timiking@gmail.com",
    //        LastName = "timiking@gmail.com",
    //        PhoneNumber = "timiking@gmail.com"
    //    };

    //    var result = await _userManager.CreateAsync(user2, "Admin123!");
    //    if (result.Succeeded)
    //    {
    //        valid = await _userManager.CheckPasswordAsync(user2, "Admin123!");
    //    }


    //    return true;
    //}
}
