namespace InventoryManagementSystem.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public ProductCategory? ParentCategory { get; set; }

        public ICollection<ProductCategory>? SubCategories { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
