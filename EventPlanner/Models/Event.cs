using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EventPlanner.Models;

public class Event
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public required string Location { get; set; }
    
    public required string OrganizerId { get; set; }
    public User Organizer { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Invitation> Invitations { get; set; } = new List<Invitation>();
}
