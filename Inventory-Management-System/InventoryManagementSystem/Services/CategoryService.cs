using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class CategoryService
    {
        private readonly InventoryContext _context;

        public CategoryService(InventoryContext context)
        {
            _context = context;
        }

        public void AddCategory(ProductCategory category)
        {
            _context.ProductCategories.Add(category);
            _context.SaveChanges();
            Console.WriteLine("Category Added!");
        }

        public List<ProductCategory> GetAllCategories()
        {
            return _context.ProductCategories.ToList();
        }

        public void ABCAnalysis()
        {
            var products = _context.Products
                .OrderByDescending(p => p.Cost)
                .ToList();

            int total = products.Count;

            for (int i = 0; i < total; i++)
            {
                if (i < total * 0.2)
                    Console.WriteLine($"A: {products[i].ProductName}");
                else if (i < total * 0.5)
                    Console.WriteLine($"B: {products[i].ProductName}");
                else
                    Console.WriteLine($"C: {products[i].ProductName}");
            }
        }
    }
}
