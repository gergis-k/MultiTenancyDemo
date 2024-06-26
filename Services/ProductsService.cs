
using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.Services;

public class ProductsService : IProductsService
{
    private readonly ApplicationDbContext context;

    public ProductsService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }
}
