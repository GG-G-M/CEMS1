using System.ComponentModel.DataAnnotations;

namespace CEMS.Models;

public class FuelPrice
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = "";

    [MaxLength(200)]
    public string Description { get; set; } = "";

    [Range(0, 9999.99)]
    public decimal Price { get; set; }

    [MaxLength(10)]
    public string Unit { get; set; } = "/L";

    [MaxLength(50)]
    public string Icon { get; set; } = "bi-droplet-fill";

    [MaxLength(30)]
    public string CssClass { get; set; } = "gasoline";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? UpdatedByUserId { get; set; }
}
