// Services/OpenSkyTokenService.cs
using System.Text.Json;
using AviTrack.Api.Settings;

namespace AviTrack.Api.Services;

public class OpenSkyTokenService
{
    private readonly HttpClient _http;
    private readonly OpenSkySettings _settings;
    private string? _cachedToken;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public OpenSkyTokenService(HttpClient http, OpenSkySettings settings)
    {
        _http = http;
        _settings = settings;
    }

    public async Task<string?> GetTokenAsync()
    {
        if (_cachedToken is not null && DateTime.UtcNow < _tokenExpiry.AddSeconds(-30))
        {
            return _cachedToken;
        }

        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://auth.opensky-network.org/auth/realms/opensky-network/protocol/openid-connect/token");

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _settings.ClientId,
            ["client_secret"] = _settings.ClientSecret
        });

        var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

        if (tokenResponse?.AccessToken is null)
        {
            return null;
        }

        _cachedToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

        return _cachedToken;
    }

    private class TokenResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}