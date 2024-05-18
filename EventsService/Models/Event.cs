global using EventsService.Models.DTO;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
global using System.ComponentModel.DataAnnotations.Schema;

namespace EventsService.Models;

public class Event
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public UserDTO Creator {  get; set; }
    [Required]
    public DateTime BeginDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public LocationType LocationType { get; set; }

    [ForeignKey("LocationId"), ValidateNever]
    public Location Location { get; set; }
    
    [ValidateNever]
    public int LocationId { get; set; }

    [ValidateNever]
    public double Latitude { get; set; }

    [ValidateNever]
    public double Longitude { get; set; }

    /// <summary>
    /// В деле
    /// </summary>
    [ValidateNever]
    public List<UserDTO> AreIn { get; set; }
}
