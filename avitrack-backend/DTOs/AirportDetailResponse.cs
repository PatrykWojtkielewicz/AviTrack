using AviTrack.Api.Services;

namespace AviTrack.Api.DTOs;

public class AirportDetailResponse
{
    public int Id { get; set; }
    public string IcaoCode { get; set; }
    public string CustomLabel { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<FlightState> NearbyFlights { get; set; }

    public AirportDetailResponse(
        int id,
        string icaoCode,
        string customLabel,
        DateTime createdAt,
        string name,
        string city,
        string country,
        double latitude,
        double longitude,
        List<FlightState> nearbyFlights
    )
    {
        Id = id;
        IcaoCode = icaoCode;
        CustomLabel = customLabel;
        CreatedAt = createdAt;
        Name = name;
        City = city;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        NearbyFlights = nearbyFlights;
    }
}