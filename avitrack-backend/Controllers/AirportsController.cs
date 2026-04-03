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

    public AirportsController(AirportService airportService)
    {
        _airportService = airportService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var airports = await _airportService.GetAll(GetUserId());
        return Ok(airports);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAirportRequest request)
    {
        var airport = await _airportService.Add(GetUserId(), request);
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