using Xunit;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

public class StockServiceTests
{
    private InventoryContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<InventoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InventoryContext(options);
    }

    [Fact]
    public void StockIn_Should_Increase_Stock()
    {
        var context = GetDbContext();

        context.Products.Add(new Product
        {
            ProductId = 1,
            SKU = "P1",
            ProductName = "Test",
            Cost = 10,
            ListPrice = 20,
            CategoryId = 1
        });

        context.Warehouses.Add(new Warehouse
        {
            WarehouseId = 1,
            WarehouseName = "Main",
            Capacity = 1000
        });

        context.SaveChanges();

        var service = new StockService(context);

        service.StockIn(1, 1, 50, "Initial");

        var stock = context.StockLevels.FirstOrDefault();

        Assert.NotNull(stock);
        Assert.Equal(50, stock.QuantityOnHand);
    }
}
