namespace InventoryManagementSystem.Models
{
    public enum TransactionType
    {
        IN,
        OUT,
        TRANSFER_IN,
        TRANSFER_OUT,
        ADJUST
    }

    public class StockTransaction
    {
        public int TransactionId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;

        public TransactionType TransactionType { get; set; }

        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string? Reference { get; set; }
    }
}
