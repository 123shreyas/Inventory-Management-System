namespace InventoryManagementSystem.Models
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }

        public ICollection<StockTransaction>? StockTransactions { get; set; }
        public ICollection<StockLevel>? StockLevels { get; set; }
    }
}
