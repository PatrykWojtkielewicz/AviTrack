namespace AviTrack.Api.Models;

public class TrackedAircraftType
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string IcaoTypeCode { get; set; } = string.Empty;
    public string CustomLabel { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}