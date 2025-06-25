namespace EventPlanner.Models;

public class Invitation
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public required string UserId { get; set; }

    public User User { get; set; } = null!;

    public required string Status { get; set; } // to-do
}
