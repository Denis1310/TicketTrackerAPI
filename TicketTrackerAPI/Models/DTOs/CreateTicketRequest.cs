using System.ComponentModel.DataAnnotations;
using TicketTrackerAPI.Entities.enums;

namespace TicketTrackerAPI.Models.DTOs;

public class CreateTicketRequest
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
    public string Title { get; set; }
    public string? Description { get; set; }

    [Required]
    [EnumDataType(typeof(Priority), ErrorMessage = "Invalid priority value.")]
    public Priority Priority { get; set; }
}
