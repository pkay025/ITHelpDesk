using ITHelpDesk.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.API.Data;

public class HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();

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
    }
}
