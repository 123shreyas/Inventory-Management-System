namespace InventoryManagementSystem.Models
{
    public class StockLevel
    {
        public int StockLevelId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;

        public int QuantityOnHand { get; set; } = 0;
        public int ReorderLevel { get; set; }
        public int SafetyStock { get; set; }
    }
}
