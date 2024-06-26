namespace MultiTenancyDemo.Settings;

public class TenantSettings
{
    public Configurations Defaults { get; set; } = default!;

    public List<Tenant> Tenants { get; set; } = [];
}
