using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Models;

namespace AviTrack.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<TrackedAirport> TrackedAirports => Set<TrackedAirport>();
    public DbSet<TrackedFlight> TrackedFlights => Set<TrackedFlight>();
}