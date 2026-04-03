namespace AviTrack.Api.DTOs;

public record AddFlightRequest(string Callsign, string CustomLabel);
public record UpdateFlightRequest(string CustomLabel);
public record FlightResponse(int Id, string Callsign, string CustomLabel, DateTime CreatedAt);