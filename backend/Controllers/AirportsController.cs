using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AviTrack.Api.DTOs;
using AviTrack.Api.Services;

namespace AviTrack.Api.Controllers;

[ApiController]
[Route("api/airports")]
[Authorize]
public class AirportsController : ControllerBase
{
    private readonly AirportService _airportService;
    private readonly AirportDataService _airportDataService;

    public AirportsController(AirportService airportService, AirportDataService airportDataService)
    {
        _airportService = airportService;
        _airportDataService = airportDataService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("search")]
    public async Task<IActionResult> SearchByCity([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Ok(Array.Empty<object>());

        var results = await _airportDataService.SearchByCity(city);
        return Ok(results);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var airports = await _airportService.GetAll(GetUserId());
        return Ok(airports);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var airport = await _airportService.GetDetailById(GetUserId(), id);

        if (airport is null)
        {
            return NotFound();
        }

        return Ok(airport);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAirportRequest request)
    {
        var airport = await _airportService.Add(GetUserId(), request);
        if (airport is null)
            return BadRequest(new { error = "Lotnisko o podanym kodzie ICAO nie istnieje" });
        return Ok(airport);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAirportRequest request)
    {
        var success = await _airportService.Update(GetUserId(), id, request);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _airportService.Delete(GetUserId(), id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}