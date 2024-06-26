using Microsoft.Extensions.Options;

namespace MultiTenancyDemo.Services;

public class TenantService : ITenantService
{
    private readonly TenantSettings tenantSettings;
    private readonly HttpContext? httpContext;

    private Tenant? currentTenant;

    public TenantService(
        IOptions<TenantSettings> options,
        IHttpContextAccessor httpContextAccessor)
    {
        tenantSettings = options.Value;
        httpContext = httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            if (httpContext.Request.Headers.TryGetValue("tenantId", out var tenantId))
            {
                currentTenant = tenantSettings.Tenants.FirstOrDefault(t => t.Id == tenantId);

                if (currentTenant is null)
                {
                    throw new InvalidOperationException("Invalid tenant id!");
                }

                if (string.IsNullOrEmpty(currentTenant.ConnectionString))
                {
                    currentTenant.ConnectionString = tenantSettings.Defaults.ConnectionString;
                }

            }
            else
            {
                throw new NotSupportedException("No tenant provided!");
            }
        }
    }

    public string? GetConnectionString()
    {
        var currentConnectionString = currentTenant is null 
            ? tenantSettings.Defaults.ConnectionString
            : currentTenant.ConnectionString;

        return currentConnectionString;
    }

    public Tenant? GetCurrentTenant()
    {
        return currentTenant;
    }

    public string? GetDbProvider()
    {
        return tenantSettings.Defaults.DbProvider;
    }
}
