using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.DTOs;
using AviTrack.Api.Models;

namespace AviTrack.Api.Services;

public class FlightService
{
    private readonly AppDbContext _db;

    public FlightService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<FlightResponse>> GetAll(int userId)
    {
        return await _db.TrackedFlights
            .Where(f => f.UserId == userId)
            .Select(f => new FlightResponse(f.Id, f.Callsign, f.CustomLabel, f.CreatedAt))
            .ToListAsync();
    }

    public async Task<FlightResponse> Add(int userId, AddFlightRequest request)
    {
        var flight = new TrackedFlight
        {
            UserId = userId,
            Callsign = request.Callsign.ToUpper(),
            CustomLabel = request.CustomLabel
        };

        _db.TrackedFlights.Add(flight);
        await _db.SaveChangesAsync();

        return new FlightResponse(flight.Id, flight.Callsign, flight.CustomLabel, flight.CreatedAt);
    }

    public async Task<bool> Update(int userId, int flightId, UpdateFlightRequest request)
    {
        var flight = await _db.TrackedFlights
            .FirstOrDefaultAsync(f => f.Id == flightId && f.UserId == userId);

        if (flight is null)
            return false;

        flight.CustomLabel = request.CustomLabel;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int userId, int flightId)
    {
        var flight = await _db.TrackedFlights
            .FirstOrDefaultAsync(f => f.Id == flightId && f.UserId == userId);

        if (flight is null)
            return false;

        _db.TrackedFlights.Remove(flight);
        await _db.SaveChangesAsync();
        return true;
    }
}