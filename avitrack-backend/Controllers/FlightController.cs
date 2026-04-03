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

    [HttpPost]
    public async Task<IActionResult> Add(AddFlightRequest request)
    {
        var flight = await _flightService.Add(GetUserId(), request);
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