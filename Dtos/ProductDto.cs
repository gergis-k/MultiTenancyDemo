using System.ComponentModel.DataAnnotations;

namespace MultiTenancyDemo.Dtos;

public class ProductDto
{
    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(64, MinimumLength = 6)]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0, 5)]
    public int Rate { get; set; }
}
