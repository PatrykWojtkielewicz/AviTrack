namespace AviTrack.Api.DTOs;

public class DashboardAirport
{
    public int Id { get; set; }
    public string IcaoCode { get; set; } = string.Empty;
    public string CustomLabel { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public List<FlightState> NearbyFlights { get; set; } = [];
}

public class DashboardFlight
{
    public int Id { get; set; }
    public string Callsign { get; set; } = string.Empty;
    public string CustomLabel { get; set; } = string.Empty;
    public FlightState? LiveData { get; set; }
}

public class DashboardAircraftType
{
    public int Id { get; set; }
    public string IcaoTypeCode { get; set; } = string.Empty;
    public string CustomLabel { get; set; } = string.Empty;
    public List<FlightState> LiveFlights { get; set; } = [];
}

public class DashboardResponse
{
    public List<DashboardAirport> Airports { get; set; } = [];
    public List<DashboardFlight> Flights { get; set; } = [];
    public List<DashboardAircraftType> AircraftTypes { get; set; } = [];
}