namespace AviTrack.Api.DTOs;

public record AddAirportRequest(string IcaoCode, string CustomLabel);
public record UpdateAirportRequest(string CustomLabel);
public record AirportResponse(int Id, string IcaoCode, string CustomLabel, DateTime CreatedAt);
