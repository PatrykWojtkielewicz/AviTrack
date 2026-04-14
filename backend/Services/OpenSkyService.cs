// Services/OpenSkyService.cs
using System.Text.Json;
using System.Net;
using System.Net.Http.Headers;
using AviTrack.Api.DTOs;
using AviTrack.Api.Exceptions;

namespace AviTrack.Api.Services;

public class OpenSkyService
{
    private readonly HttpClient _http;
    private readonly OpenSkyTokenService _tokenService;

    public OpenSkyService(HttpClient http, OpenSkyTokenService tokenService)
    {
        _http = http;
        _tokenService = tokenService;
    }

    public async Task<List<FlightState>> GetFlightsInArea(double lamin, double lomin, double lamax, double lomax)
    {
        var url = $"https://opensky-network.org/api/states/all?lamin={lamin}&lomin={lomin}&lamax={lamax}&lomax={lomax}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        await AttachTokenAsync(request);

        var response = await _http.SendAsync(request);

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

    public async Task<FlightState?> GetFlightByCallsign(string callsign)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://opensky-network.org/api/states/all");
        await AttachTokenAsync(request);

        var response = await _http.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<OpenSkyResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data?.States is null)
            return null;

        return data.States
            .Select(ParseState)
            .FirstOrDefault(s => s?.Callsign.Equals(callsign, StringComparison.OrdinalIgnoreCase) == true);
    }

    private async Task AttachTokenAsync(HttpRequestMessage request)
    {
        var token = await _tokenService.GetTokenAsync();
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
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
}