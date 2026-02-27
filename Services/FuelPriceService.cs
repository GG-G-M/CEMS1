using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CEMS.Services;

public class FuelPriceDto
{
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
    public decimal Price { get; set; }
    public string Unit { get; set; } = "/L";
    public string Icon { get; set; } = "bi-droplet-fill";
    public string Cls { get; set; } = "gasoline";
    public string Trend { get; set; } = "stable";
    public string Change { get; set; } = "0.00";
}

public class FuelPriceService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;
    private readonly ILogger<FuelPriceService> _logger;
    private const string CacheKey = "FuelPrices";

    public FuelPriceService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IConfiguration config,
        ILogger<FuelPriceService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _config = config;
        _logger = logger;
    }

    public async Task<List<FuelPriceDto>> GetFuelPricesAsync()
    {
        if (_cache.TryGetValue(CacheKey, out List<FuelPriceDto>? cached) && cached is not null)
        {
            return cached;
        }

        List<FuelPriceDto>? prices = null;

        // Try external API if configured
        var apiUrl = _config["FuelPriceApi:Url"];
        if (!string.IsNullOrWhiteSpace(apiUrl))
        {
            prices = await FetchFromExternalApiAsync(apiUrl);
        }

        // Fallback to DOE reference prices
        prices ??= GetDoeReferencePrices();

        var expiry = TimeSpan.FromMinutes(
            _config.GetValue("FuelPriceApi:CacheMinutes", 60));
        _cache.Set(CacheKey, prices, expiry);

        return prices;
    }

    private async Task<List<FuelPriceDto>?> FetchFromExternalApiAsync(string apiUrl)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var apiKey = _config["FuelPriceApi:ApiKey"];
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"apikey {apiKey}");
            }

            var response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Fuel price API returned {StatusCode}", response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<FuelPriceDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is { Count: > 0 })
            {
                _logger.LogInformation("Fetched {Count} fuel prices from external API", result.Count);
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch fuel prices from external API");
        }

        return null;
    }

    /// <summary>
    /// DOE Philippines reference fuel prices (Metro Manila prevailing range midpoints).
    /// Update these periodically or replace with a live API source via appsettings.
    /// </summary>
    private static List<FuelPriceDto> GetDoeReferencePrices() =>
    [
        new() { Name = "Unleaded 91",    Desc = "Regular Gasoline (91 RON)",   Price = 57.50m, Icon = "bi-droplet-fill",           Cls = "gasoline",   Trend = "up",     Change = "+0.75" },
        new() { Name = "Premium 95",     Desc = "Premium Gasoline (95 RON)",   Price = 62.75m, Icon = "bi-droplet-fill",           Cls = "premium",    Trend = "up",     Change = "+0.50" },
        new() { Name = "High Octane 97+",Desc = "High Performance (97+ RON)",  Price = 72.30m, Icon = "bi-lightning-charge-fill",  Cls = "highoctane", Trend = "down",   Change = "-0.25" },
        new() { Name = "Diesel",         Desc = "Common for trucks & SUVs",    Price = 51.85m, Icon = "bi-truck",                  Cls = "diesel",     Trend = "down",   Change = "-1.10" },
        new() { Name = "Premium Diesel", Desc = "Enhanced diesel fuel",        Price = 56.40m, Icon = "bi-truck",                  Cls = "diesel",     Trend = "stable", Change = "0.00"  },
        new() { Name = "Kerosene",       Desc = "Industrial & aviation use",   Price = 55.90m, Icon = "bi-fire",                   Cls = "kerosene",   Trend = "up",     Change = "+0.30" },
        new() { Name = "LPG (Autogas)",  Desc = "Liquefied Petroleum Gas",     Price = 28.50m, Icon = "bi-cloud-fill",             Cls = "lpg",        Trend = "stable", Change = "0.00"  },
        new() { Name = "E10 Biofuel",    Desc = "Ethanol-blended gasoline",    Price = 55.20m, Icon = "bi-leaf-fill",              Cls = "bio",        Trend = "down",   Change = "-0.40" },
    ];
}
