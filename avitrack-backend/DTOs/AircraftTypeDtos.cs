namespace AviTrack.Api.DTOs;

public record AddAircraftTypeRequest(string IcaoTypeCode, string CustomLabel);
public record UpdateAircraftTypeRequest(string CustomLabel);
public record AircraftTypeResponse(int Id, string IcaoTypeCode, string CustomLabel, DateTime CreatedAt);