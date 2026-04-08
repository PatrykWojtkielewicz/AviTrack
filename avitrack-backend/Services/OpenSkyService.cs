using System.Text.Json;
using System.Net;
using AviTrack.Api.DTOs;
using AviTrack.Api.Exceptions;

namespace AviTrack.Api.Services;

public class OpenSkyService
{
    private readonly HttpClient _http;

    public OpenSkyService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<FlightState>> GetFlightsInArea(double lamin, double lomin, double lamax, double lomax)
    {
        var url = $"https://opensky-network.org/api/states/all?lamin={lamin}&lomin={lomin}&lamax={lamax}&lomax={lomax}";

        var response = await _http.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new TooManyRequestsException();
        }

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<OpenSkyResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data?.States is null)
            return [];

        return data.States
            .Select(ParseState)
            .Where(s => s is not null)
            .Select(s => s!)
            .ToList();
    }

    private FlightState? ParseState(List<object> state)
    {
        if (state.Count < 9)
            return null;

        return new FlightState
        {
            Icao24 = state[0]?.ToString()?.Trim() ?? string.Empty,
            Callsign = state[1]?.ToString()?.Trim() ?? string.Empty,
            OriginCountry = state[2]?.ToString() ?? string.Empty,
            Longitude = state[5] is JsonElement lon ? lon.GetDouble() : null,
            Latitude = state[6] is JsonElement lat ? lat.GetDouble() : null,
            Altitude = state[7] is JsonElement alt ? alt.GetDouble() : null,
            OnGround = state[8] is JsonElement og && og.GetBoolean(),
            Velocity = state[9] is JsonElement vel ? vel.GetDouble() : null,
            Heading = state[10] is JsonElement hdg ? hdg.GetDouble() : null,
        };
    }

    public async Task<FlightState?> GetFlightByCallsign(string callsign)
    {
        var response = await _http.GetStringAsync("https://opensky-network.org/api/states/all");
        var data = JsonSerializer.Deserialize<OpenSkyResponse>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data?.States is null)
            return null;

        return data.States
            .Select(ParseState)
            .FirstOrDefault(s => s?.Callsign.Equals(callsign, StringComparison.OrdinalIgnoreCase) == true);
    }

    public async Task<List<FlightState>> GetFlightsByAircraftType(string icaoTypeCode)
    {
        var response = await _http.GetStringAsync("https://opensky-network.org/api/states/all");
        var data = JsonSerializer.Deserialize<OpenSkyResponse>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data?.States is null)
            return [];

        return data.States
            .Select(ParseState)
            .Where(s => s is not null)
            .Select(s => s!)
            .ToList();
    }
}