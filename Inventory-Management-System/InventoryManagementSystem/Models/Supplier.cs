namespace InventoryManagementSystem.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }

        public ICollection<ProductSupplier>? ProductSuppliers { get; set; }
    }
}
