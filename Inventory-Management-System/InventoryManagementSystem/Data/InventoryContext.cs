using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Data
{
    public class InventoryContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockLevel> StockLevels { get; set; }

        // ✅ Constructor for dependency injection / unit tests
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        // Optional default config for console app
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseMySql(
        //             "server=localhost;database=InventoryDB;user=root;password=Shreyas@123",
        //             new MySqlServerVersion(new Version(8, 0, 36))
        //         );
        //     }
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => pc.ProductCategoryId);

            modelBuilder.Entity<StockTransaction>()
            .HasKey(st => st.TransactionId);



            // Product -> Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductSupplier
            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId);

            // StockLevel
            modelBuilder.Entity<StockLevel>()
                .HasOne(sl => sl.Product)
                .WithMany(p => p.StockLevels)
                .HasForeignKey(sl => sl.ProductId);

            modelBuilder.Entity<StockLevel>()
                .HasOne(sl => sl.Warehouse)
                .WithMany(w => w.StockLevels)
                .HasForeignKey(sl => sl.WarehouseId);

            // StockTransaction
            modelBuilder.Entity<StockTransaction>()
                .HasOne(st => st.Product)
                .WithMany(p => p.StockTransactions)
                .HasForeignKey(st => st.ProductId);

            modelBuilder.Entity<StockTransaction>()
                .HasOne(st => st.Warehouse)
                .WithMany(w => w.StockTransactions)
                .HasForeignKey(st => st.WarehouseId);

            // Self reference (Category hierarchy)
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.ParentCategory)
                .WithMany(pc => pc.SubCategories)
                .HasForeignKey(pc => pc.ParentCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unique SKU
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // Enum stored as string
            modelBuilder.Entity<StockTransaction>()
                .Property(s => s.TransactionType)
                .HasConversion<string>();
        }


    }
}
