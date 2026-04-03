using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.DTOs;
using AviTrack.Api.Models;

namespace AviTrack.Api.Services;

public class AircraftTypeService
{
    private readonly AppDbContext _db;

    public AircraftTypeService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AircraftTypeResponse>> GetAll(int userId)
    {
        return await _db.TrackedAircraftTypes
            .Where(a => a.UserId == userId)
            .Select(a => new AircraftTypeResponse(a.Id, a.IcaoTypeCode, a.CustomLabel, a.CreatedAt))
            .ToListAsync();
    }

    public async Task<AircraftTypeResponse> Add(int userId, AddAircraftTypeRequest request)
    {
        var aircraft = new TrackedAircraftType
        {
            UserId = userId,
            IcaoTypeCode = request.IcaoTypeCode.ToLower(),
            CustomLabel = request.CustomLabel
        };

        _db.TrackedAircraftTypes.Add(aircraft);
        await _db.SaveChangesAsync();

        return new AircraftTypeResponse(aircraft.Id, aircraft.IcaoTypeCode, aircraft.CustomLabel, aircraft.CreatedAt);
    }

    public async Task<bool> Update(int userId, int aircraftId, UpdateAircraftTypeRequest request)
    {
        var aircraft = await _db.TrackedAircraftTypes
            .FirstOrDefaultAsync(a => a.Id == aircraftId && a.UserId == userId);

        if (aircraft is null)
            return false;

        aircraft.CustomLabel = request.CustomLabel;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int userId, int aircraftId)
    {
        var aircraft = await _db.TrackedAircraftTypes
            .FirstOrDefaultAsync(a => a.Id == aircraftId && a.UserId == userId);

        if (aircraft is null)
            return false;

        _db.TrackedAircraftTypes.Remove(aircraft);
        await _db.SaveChangesAsync();
        return true;
    }
}