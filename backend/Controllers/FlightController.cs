using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AviTrack.Api.DTOs;
using AviTrack.Api.Services;

namespace AviTrack.Api.Controllers;

[ApiController]
[Route("api/flights")]
[Authorize]
public class FlightsController : ControllerBase
{
    private readonly FlightService _flightService;

    public FlightsController(FlightService flightService)
    {
        _flightService = flightService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var flights = await _flightService.GetAll(GetUserId());
        return Ok(flights);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var type = await _flightService.GetById(GetUserId(), id);

        if (type is null)
        {
            return NotFound();
        }

        return Ok(type);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddFlightRequest request)
    {
        if (await _flightService.IsFlightTracked(GetUserId(), request.Callsign))
            return BadRequest(new { error = "Ten lot jest już śledzony" });

        var flight = await _flightService.Add(GetUserId(), request);
        if (flight is null)
            return BadRequest(new { error = "Lot o podanym callsign nie istnieje" });
        return Ok(flight);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFlightRequest request)
    {
        var success = await _flightService.Update(GetUserId(), id, request);
        if (!success)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _flightService.Delete(GetUserId(), id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}