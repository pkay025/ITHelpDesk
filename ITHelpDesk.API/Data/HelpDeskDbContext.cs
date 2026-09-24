using ITHelpDesk.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.API.Data;

public class HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(ticket => ticket.Title).HasMaxLength(120).IsRequired();
            entity.Property(ticket => ticket.Description).HasMaxLength(4000).IsRequired();
            entity.Property(ticket => ticket.RequesterName).HasMaxLength(120).IsRequired();
            entity.Property(ticket => ticket.RequesterEmail).HasMaxLength(320).IsRequired();
            entity.Property(ticket => ticket.AssignedTo).HasMaxLength(120);
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.Property(comment => comment.AuthorName).HasMaxLength(120).IsRequired();
            entity.Property(comment => comment.AuthorEmail).HasMaxLength(320).IsRequired();
            entity.Property(comment => comment.Message).HasMaxLength(4000).IsRequired();
            entity.HasIndex(comment => new { comment.TicketId, comment.CreatedAtUtc });
        });
    }
}
