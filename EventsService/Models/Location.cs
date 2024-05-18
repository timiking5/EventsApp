namespace EventsService.Models;

public class Location
{
    [Key]
    public int Id { get; set; }
 
    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
}
