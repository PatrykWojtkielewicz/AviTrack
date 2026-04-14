using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Models;

namespace AviTrack.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<TrackedAirport> TrackedAirports => Set<TrackedAirport>();
    public DbSet<TrackedFlight> TrackedFlights => Set<TrackedFlight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<TrackedAirport>()
            .HasIndex(a => new { a.UserId, a.IcaoCode }).IsUnique();

        modelBuilder.Entity<TrackedFlight>()
            .HasIndex(f => new { f.UserId, f.Callsign }).IsUnique();
    }
}