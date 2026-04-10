using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.DTOs;

namespace AviTrack.Api.Services;

public class DashboardService
{
    private readonly AppDbContext _db;
    private readonly OpenSkyService _openSky;
    private readonly AirportDataService _airportData;

    public DashboardService(AppDbContext db, OpenSkyService openSky, AirportDataService airportData)
    {
        _db = db;
        _openSky = openSky;
        _airportData = airportData;
    }

    public async Task<DashboardResponse> GetDashboard(int userId)
    {
        var response = new DashboardResponse();

        var trackedAirports = await _db.TrackedAirports
            .Where(a => a.UserId == userId)
            .ToListAsync();

        var trackedFlights = await _db.TrackedFlights
            .Where(f => f.UserId == userId)
            .ToListAsync();

        foreach (var airport in trackedAirports)
        {
            var info = await _airportData.GetByIcao(airport.IcaoCode);
            var dashboardAirport = new DashboardAirport
            {
                Id = airport.Id,
                IcaoCode = airport.IcaoCode,
                CustomLabel = airport.CustomLabel,
                Name = info?.Name ?? airport.IcaoCode,
                City = info?.City ?? string.Empty,
                Country = info?.Country ?? string.Empty,
                Latitude = info?.Latitude,
                Longitude = info?.Longitude
            };

            if (info is not null)
            {
                var delta = 1.0;
                dashboardAirport.NearbyFlights = await _openSky.GetFlightsInArea(
                    info.Latitude - delta,
                    info.Longitude - delta,
                    info.Latitude + delta,
                    info.Longitude + delta
                );
            }

            response.Airports.Add(dashboardAirport);
        }

        foreach (var flight in trackedFlights)
        {
            var liveData = await _openSky.GetFlightByCallsign(flight.Callsign);
            response.Flights.Add(new DashboardFlight
            {
                Id = flight.Id,
                Callsign = flight.Callsign,
                CustomLabel = flight.CustomLabel,
                LiveData = liveData
            });
        }

        return response;
    }
}