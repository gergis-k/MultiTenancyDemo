namespace MultiTenancyDemo.Models;

public class Product : ITenantable
{
    public int Id { get; set; }

    public string TenantId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Rate { get; set; }
}
