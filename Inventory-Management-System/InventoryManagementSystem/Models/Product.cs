namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? SKU { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int CategoryId { get; set; }
public ProductCategory? Category { get; set; }


        public string? UnitOfMeasure { get; set; }
        public decimal Cost { get; set; }
        public decimal ListPrice { get; set; }
        public bool IsActive { get; set; } = true;
        // public int ReorderLevel { get; set; }

        public ICollection<ProductSupplier>? ProductSuppliers { get; set; }
        public ICollection<StockTransaction>? StockTransactions { get; set; }
        public ICollection<StockLevel>? StockLevels { get; set; }
    }
}
