using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.DTOs;
using AviTrack.Api.Models;

namespace AviTrack.Api.Services;

public class AirportService
{
    private readonly AppDbContext _db;

    public AirportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AirportResponse>> GetAll(int userId)
    {
        return await _db.TrackedAirports
            .Where(a => a.User.Id == userId)
            .Select(a => new AirportResponse(a.Id, a.IcaoCode, a.CustomLabel, a.CreatedAt))
            .ToListAsync();
    }

    public async Task<AirportResponse> Add(int userId, AddAirportRequest request)
    {
        var airport = new TrackedAirport
        {
            UserId = userId,
            IcaoCode = request.IcaoCode.ToUpper(),
            CustomLabel = request.CustomLabel
        };

        _db.TrackedAirports.Add(airport);
        await _db.SaveChangesAsync();

        return new AirportResponse(airport.Id, airport.IcaoCode, airport.CustomLabel, airport.CreatedAt);
    }

    public async Task<bool> Update(int userId, int airportId, UpdateAirportRequest request)
    {
        var airport = await _db.TrackedAirports
            .FirstOrDefaultAsync(a => a.Id == airportId && a.UserId == userId);

        if (airport is null)
            return false;

        airport.CustomLabel = request.CustomLabel;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int userId, int airportId)
    {
        var airport = await _db.TrackedAirports
            .FirstOrDefaultAsync(a => a.Id == airportId && a.UserId == userId);

        if (airport is null)
            return false;

        _db.TrackedAirports.Remove(airport);
        await _db.SaveChangesAsync();
        return true;
    }
}