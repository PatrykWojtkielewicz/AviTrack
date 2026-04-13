using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.DTOs;
using AviTrack.Api.Models;

namespace AviTrack.Api.Services;

public class AirportService
{
    private readonly AppDbContext _db;
    private readonly AirportDataService _airportData;
    private readonly OpenSkyService _openSky;

    public AirportService(AppDbContext db, AirportDataService airportData, OpenSkyService openSky)
    {
        _db = db;
        _airportData = airportData;
        _openSky = openSky;
    }

    public async Task<List<AirportResponse>> GetAll(int userId)
    {
        return await _db.TrackedAirports
            .Where(a => a.User.Id == userId)
            .Select(a => new AirportResponse(a.Id, a.IcaoCode, a.CustomLabel, a.CreatedAt))
            .ToListAsync();
    }

    public async Task<AirportResponse?> GetById(int userId, int airportId)
    {
        return await _db.TrackedAirports
            .Where(a => a.Id == airportId && a.UserId == userId)
            .Select(a => new AirportResponse(a.Id, a.IcaoCode, a.CustomLabel, a.CreatedAt))
            .FirstOrDefaultAsync();
    }

    public async Task<AirportDetailResponse?> GetDetailById(int userId, int airportId)
    {
        var airport = await _db.TrackedAirports
            .Where(a => a.Id == airportId && a.UserId == userId)
            .FirstOrDefaultAsync();

        if (airport is null)
            return null;

        var airportInfo = await _airportData.GetByIcao(airport.IcaoCode);

        if (airportInfo is null)
            return null;

        var delta = 1.0;
        var nearbyFlights = await _openSky.GetFlightsInArea(
            airportInfo.Latitude - delta,
            airportInfo.Longitude - delta,
            airportInfo.Latitude + delta,
            airportInfo.Longitude + delta
        );

        return new AirportDetailResponse(
            airport.Id,
            airport.IcaoCode,
            airport.CustomLabel,
            airport.CreatedAt,
            airportInfo.Name,
            airportInfo.City,
            airportInfo.Country,
            airportInfo.Latitude,
            airportInfo.Longitude,
            nearbyFlights
        );
    }

    public async Task<bool> IsAirportTracked(int userId, string icaoCode)
    {
        return await _db.TrackedAirports
            .AnyAsync(a => a.UserId == userId && a.IcaoCode == icaoCode.ToUpper());
    }

    public async Task<AirportResponse?> Add(int userId, AddAirportRequest request)
    {
        var icaoCode = request.IcaoCode.ToUpper();

        var existingAirport = await _db.TrackedAirports
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IcaoCode == icaoCode);
        if (existingAirport is not null)
            return null;

        var airportInfo = await _airportData.GetByIcao(icaoCode);
        if (airportInfo is null)
            return null;

        var airport = new TrackedAirport
        {
            UserId = userId,
            IcaoCode = icaoCode,
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