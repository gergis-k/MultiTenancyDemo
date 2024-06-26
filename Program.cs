using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
        builder.Services.AddScoped(typeof(ITenantService), typeof(TenantService));
        builder.Services.AddScoped(typeof(IProductsService), typeof(ProductsService));
        builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));

        var tenantSettings = new TenantSettings();
        builder.Configuration.GetSection(nameof(TenantSettings)).Bind(tenantSettings);

        var defaultDbProvider = tenantSettings.Defaults.DbProvider;

        if (defaultDbProvider.ToLower() == "mssql")
        {
            builder.Services.AddDbContext<ApplicationDbContext>(b => b.UseSqlServer());
        }

        foreach (var tenant in tenantSettings.Tenants)
        {
            var connectionString = tenant.ConnectionString ?? tenantSettings.Defaults.ConnectionString;

            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.SetConnectionString(connectionString);

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }

        builder.Services.AddControllers();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
