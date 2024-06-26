using Microsoft.AspNetCore.Mvc;
using MultiTenancyDemo.Dtos;

namespace MultiTenancyDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService productsService;

    public ProductsController(IProductsService productsService)
    {
        this.productsService = productsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await productsService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await productsService.GetAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(ProductDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Rate = model.Rate,
        };

        var createdProduct = await productsService.CreateAsync(product);

        return Ok(createdProduct);
    }
}
