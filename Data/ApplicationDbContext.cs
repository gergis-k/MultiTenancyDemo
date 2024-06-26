using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ITenantService tenantService;

    public string TenantId { get; set; }

    public ApplicationDbContext(ITenantService tenantService)
    {
        this.tenantService = tenantService;
        TenantId = this.tenantService.GetCurrentTenant()?.Id;
    }

    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        this.tenantService = tenantService;
        TenantId = this.tenantService.GetCurrentTenant()?.Id;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = tenantService.GetConnectionString();

        if (!string.IsNullOrEmpty(tenantConnectionString))
        {
            var dbProvider = tenantService.GetDbProvider();

            if (dbProvider?.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == TenantId);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<ITenantable>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.TenantId = TenantId!;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
