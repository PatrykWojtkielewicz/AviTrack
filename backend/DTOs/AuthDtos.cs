namespace AviTrack.Api.DTOs;

public record RegisterRequest(string Username, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Username);
public record UpdateUsernameRequest(string Username);