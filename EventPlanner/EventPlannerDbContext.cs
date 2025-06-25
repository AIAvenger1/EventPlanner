using Microsoft.EntityFrameworkCore;
using EventPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventPlanner;

public class EventPlannerDbContext : IdentityDbContext<User>
{
    public EventPlannerDbContext() { }
    public EventPlannerDbContext(DbContextOptions<EventPlannerDbContext> options) : base(options) { }
    public DbSet<Event> Events { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Invitation> Invations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(u =>
        {
            u.Property(x => x.RegisteredAt)
             .HasDefaultValueSql("GETDATE()");
            u.Property(x => x.FirstName).HasMaxLength(50);
            u.Property(x => x.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Event>(e =>
        {
            e.HasOne(ev => ev.Organizer)
             .WithMany(u => u.Events)
             .HasForeignKey(ev => ev.OrganizerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ev => ev.Category)
             .WithMany(c => c.Events)
             .HasForeignKey(ev => ev.CategoryId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(c =>
        {
            c.Property(x => x.PostedAt).HasDefaultValueSql("GETDATE()");
            c.Property(x => x.Text).HasMaxLength(200);

            c.HasOne(cm => cm.Event)
             .WithMany(ev => ev.Comments)
             .HasForeignKey(cm => cm.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            c.HasOne(cm => cm.User)
             .WithMany(u => u.Comments)
             .HasForeignKey(cm => cm.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Invitation>(i =>
        {
            i.HasOne(inv => inv.Event)
             .WithMany(ev => ev.Invitations)
             .HasForeignKey(inv => inv.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            i.HasOne(inv => inv.User)
             .WithMany(u => u.Invitations)
             .HasForeignKey(inv => inv.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
