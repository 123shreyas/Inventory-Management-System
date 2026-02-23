using Xunit;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

public class ProductServiceTests
{
    private InventoryContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<InventoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InventoryContext(options);
    }

    [Fact]
    public void AddProduct_Should_Add_Product()
    {
        var context = GetDbContext();
        var service = new ProductService(context);

        var product = new Product
        {
            SKU = "TEST123",
            ProductName = "Test Product",
            Cost = 10,
            ListPrice = 20,
            CategoryId = 1
        };

        service.AddProduct(product);

        Assert.Equal(1, context.Products.Count());
    }

    [Fact]
    public void AddProduct_Should_Not_Add_Duplicate_SKU()
    {
        var context = GetDbContext();
        var service = new ProductService(context);

        var product1 = new Product
        {
            SKU = "DUP123",
            ProductName = "Product 1",
            Cost = 10,
            ListPrice = 20,
            CategoryId = 1
        };

        var product2 = new Product
        {
            SKU = "DUP123",
            ProductName = "Product 2",
            Cost = 15,
            ListPrice = 25,
            CategoryId = 1
        };

        service.AddProduct(product1);
        service.AddProduct(product2);

        Assert.Equal(1, context.Products.Count());
    }
}
