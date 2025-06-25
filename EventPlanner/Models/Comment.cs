namespace EventPlanner.Models;

public class Comment
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public required string UserId   { get; set; } 
    public User User { get; set; } = null!;
    public required string Text { get; set; }
    public DateTime PostedAt { get; set; }
}
