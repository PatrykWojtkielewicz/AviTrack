namespace AviTrack.Api.Services;

public class AirportInfo
{
    public string Icao { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class AirportDataService
{
    private readonly HttpClient _http;
    private Dictionary<string, AirportInfo> _airports = new();
    private bool _loaded = false;

    public AirportDataService(HttpClient http)
    {
        _http = http;
    }

    public async Task EnsureLoadedAsync()
    {
        if (_loaded)
            return;

        var csv = await _http.GetStringAsync("https://raw.githubusercontent.com/jpatokal/openflights/master/data/airports.dat");

        foreach (var line in csv.Split('\n'))
        {
            var parts = line.Split(',');
            if (parts.Length < 8)
                continue;

            var icao = parts[5].Trim('"');
            if (icao.Length != 4)
                continue;

            if (!double.TryParse(parts[6].Trim('"'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat))
                continue;

            if (!double.TryParse(parts[7].Trim('"'), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
                continue;

            _airports[icao.ToUpper()] = new AirportInfo
            {
                Icao = icao.ToUpper(),
                Name = parts[1].Trim('"'),
                City = parts[2].Trim('"'),
                Country = parts[3].Trim('"'),
                Latitude = lat,
                Longitude = lon
            };
        }

        _loaded = true;
    }

    public async Task<AirportInfo?> GetByIcao(string icao)
    {
        await EnsureLoadedAsync();
        _airports.TryGetValue(icao.ToUpper(), out var info);
        return info;
    }
}