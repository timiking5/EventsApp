using AuthService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace AuthService.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{

}
