namespace AviTrack.Api.DTOs;

public class OpenSkyResponse
{
    public long Time { get; set; }
    public List<List<object>>? States { get; set; }
}

public class FlightState
{
    public string Icao24 { get; set; } = string.Empty;
    public string Callsign { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Altitude { get; set; }
    public double? Velocity { get; set; }
    public double? Heading { get; set; }
    public bool OnGround { get; set; }
}