namespace AviTrack.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Emial { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;
}