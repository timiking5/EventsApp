using EventsService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// События
    /// </summary>
    public DbSet<Event> Events { get; set; }

    /// <summary>
    /// Места проведения событий
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Пользователи. Могут подписываться/создавать события. Приходят из AuthService'a с пом. MQ
    /// </summary>
    public DbSet<UserDTO> Users { get; set; }
}
