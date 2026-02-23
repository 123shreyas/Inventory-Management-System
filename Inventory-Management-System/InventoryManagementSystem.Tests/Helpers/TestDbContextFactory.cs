using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;

public static class TestDbContextFactory
{
    public static InventoryContext Create()
    {
        var options = new DbContextOptionsBuilder<InventoryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InventoryContext(options);
    }
}
