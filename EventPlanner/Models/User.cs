using Microsoft.AspNetCore.Identity;

namespace EventPlanner.Models;

public class User : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public required string Role { get; set; } = "User"; // to-do

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public List<Event> Events { get; set; } = new List<Event>();

    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<Invitation> Invitations { get; set; } = new List<Invitation>();
}
