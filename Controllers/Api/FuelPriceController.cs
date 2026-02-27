using CEMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CEMS.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FuelPriceController : ControllerBase
{
    private readonly FuelPriceService _fuelPriceService;

    public FuelPriceController(FuelPriceService fuelPriceService)
    {
        _fuelPriceService = fuelPriceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFuelPrices()
    {
        var prices = await _fuelPriceService.GetFuelPricesAsync();
        return Ok(prices);
    }
}
