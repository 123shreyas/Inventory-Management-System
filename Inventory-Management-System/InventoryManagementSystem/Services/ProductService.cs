using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class ProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        // CREATE
        public void AddProduct(Product product)
        {
            var existingProduct = _context.Products
                .FirstOrDefault(p => p.SKU == product.SKU);

            if (existingProduct != null)
            {
                Console.WriteLine("Product with this SKU already exists!");
                return;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            Console.WriteLine("Product Added Successfully!");
        }


        // READ ALL
        public List<Product> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        // READ BY ID
        public Product? GetProduct(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
        }

        // UPDATE
        public bool UpdateProduct(Product product)
        {
            var existing = _context.Products.Find(product.ProductId);

            if (existing == null)
                return false;

            existing.SKU = product.SKU;
            existing.ProductName = product.ProductName;
            existing.Description = product.Description;
            existing.CategoryId = product.CategoryId;
            existing.UnitOfMeasure = product.UnitOfMeasure;
            existing.Cost = product.Cost;
            existing.ListPrice = product.ListPrice;
            existing.IsActive = product.IsActive;

            _context.SaveChanges();
            return true;
        }


        // DELETE
        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                product.IsActive = false;
                _context.SaveChanges();
                Console.WriteLine("Product Deactivated Successfully!");
            }
        }

    }
}
