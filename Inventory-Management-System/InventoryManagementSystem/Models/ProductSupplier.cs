namespace InventoryManagementSystem.Models
{
    public class ProductSupplier
    {
        public int ProductSupplierId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public string? SupplierSKU { get; set; }
        public int LeadTime { get; set; }
    }
}
