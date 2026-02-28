using Microsoft.EntityFrameworkCore;
using CEMS.Data;
using CEMS.Models;

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
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FuelPriceService> _logger;

    public FuelPriceService(
        IServiceProvider serviceProvider,
        ILogger<FuelPriceService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<List<FuelPriceDto>> GetFuelPricesAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var fuelPrices = await db.FuelPrices
                .OrderBy(f => f.Name)
                .ToListAsync();

            if (fuelPrices.Count > 0)
            {
                return fuelPrices.Select(f => new FuelPriceDto
                {
                    Name = f.Name,
                    Desc = f.Description,
                    Price = f.Price,
                    Unit = f.Unit,
                    Icon = f.Icon,
                    Cls = f.CssClass,
                    Trend = "stable",
                    Change = "0.00"
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch fuel prices from database");
        }

        return GetDefaultPrices();
    }

    /// <summary>
    /// Fallback default prices used only when the database has no entries yet.
    /// </summary>
    private static List<FuelPriceDto> GetDefaultPrices() =>
    [
        new() { Name = "Unleaded 91",    Desc = "Regular Gasoline (91 RON)",   Price = 57.50m, Icon = "bi-droplet-fill",           Cls = "gasoline",   Trend = "stable", Change = "0.00" },
        new() { Name = "Premium 95",     Desc = "Premium Gasoline (95 RON)",   Price = 62.75m, Icon = "bi-droplet-fill",           Cls = "premium",    Trend = "stable", Change = "0.00" },
        new() { Name = "High Octane 97+",Desc = "High Performance (97+ RON)",  Price = 72.30m, Icon = "bi-lightning-charge-fill",  Cls = "highoctane", Trend = "stable", Change = "0.00" },
        new() { Name = "Diesel",         Desc = "Common for trucks & SUVs",    Price = 51.85m, Icon = "bi-truck",                  Cls = "diesel",     Trend = "stable", Change = "0.00" },
        new() { Name = "Premium Diesel", Desc = "Enhanced diesel fuel",        Price = 56.40m, Icon = "bi-truck",                  Cls = "diesel",     Trend = "stable", Change = "0.00" },
        new() { Name = "Kerosene",       Desc = "Industrial & aviation use",   Price = 55.90m, Icon = "bi-fire",                   Cls = "kerosene",   Trend = "stable", Change = "0.00" },
        new() { Name = "LPG (Autogas)",  Desc = "Liquefied Petroleum Gas",     Price = 28.50m, Icon = "bi-cloud-fill",             Cls = "lpg",        Trend = "stable", Change = "0.00" },
        new() { Name = "E10 Biofuel",    Desc = "Ethanol-blended gasoline",    Price = 55.20m, Icon = "bi-leaf-fill",              Cls = "bio",        Trend = "stable", Change = "0.00" },
    ];
}
