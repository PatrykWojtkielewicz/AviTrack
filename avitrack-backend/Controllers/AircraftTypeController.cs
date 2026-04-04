using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AviTrack.Api.DTOs;
using AviTrack.Api.Services;

namespace AviTrack.Api.Controllers;

[ApiController]
[Route("api/aircraft-types")]
[Authorize]
public class AircraftTypesController : ControllerBase
{
    private readonly AircraftTypeService _aircraftTypeService;

    public AircraftTypesController(AircraftTypeService aircraftTypeService)
    {
        _aircraftTypeService = aircraftTypeService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var types = await _aircraftTypeService.GetAll(GetUserId());
        return Ok(types);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var type = await _aircraftTypeService.GetById(GetUserId(), id);

        if (type is null)
        {
            return NotFound();
        }

        return Ok(type);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAircraftTypeRequest request)
    {
        var type = await _aircraftTypeService.Add(GetUserId(), request);
        return Ok(type);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAircraftTypeRequest request)
    {
        var success = await _aircraftTypeService.Update(GetUserId(), id, request);
        if (!success)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _aircraftTypeService.Delete(GetUserId(), id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}