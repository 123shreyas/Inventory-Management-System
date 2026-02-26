using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.ConsoleUI
{
    public class CategoryConsole
    {
        private readonly CategoryService _categoryService;

        public CategoryConsole(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public void ShowMenu()
        {
            while (true)
            {
               
                Console.WriteLine("\n--- CATEGORIES ---");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. View All Categories");
                Console.WriteLine("3. ABC Analysis");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddCategory();
                        break;

                    case "2":
                        ViewAllCategories();
                        break;

                    case "3":
                        _categoryService.ABCAnalysis();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddCategory()
        {
            Console.Write("Enter Category Name: ");
            string name = Console.ReadLine()!;

            Console.Write("Enter Description (optional): ");
            string? description = Console.ReadLine();

            Console.Write("Enter Parent Category ID (or press Enter if none): ");
            string? parentInput = Console.ReadLine();

            int? parentId = null;

            if (!string.IsNullOrWhiteSpace(parentInput))
            {
                if (int.TryParse(parentInput, out int parsedId))
                    parentId = parsedId;
                else
                {
                    Console.WriteLine("Invalid Parent ID.");
                    return;
                }
            }

            var category = new ProductCategory
            {
                CategoryName = name,
                Description = description,
                ParentCategoryId = parentId
            };

            _categoryService.AddCategory(category);
        }

        private void ViewAllCategories()
        {
            var categories = _categoryService.GetAllCategories();

            if (!categories.Any())
            {
                Console.WriteLine("No categories found.");
                return;
            }

            foreach (var c in categories)
            {
                Console.WriteLine(
                    $"ID: {c.ProductCategoryId}, " +
                    $"Name: {c.CategoryName}, " +
                    $"Description: {c.Description}, " +
                    $"ParentID: {c.ParentCategoryId}"
                );
            }
        }
    }
}