namespace MultiTenancyDemo.Services;

public interface IProductsService
{
    Task<Product> CreateAsync(Product product);

    Task<Product?> GetAsync(int id);

    Task<IReadOnlyList<Product>> GetAllAsync();
}
