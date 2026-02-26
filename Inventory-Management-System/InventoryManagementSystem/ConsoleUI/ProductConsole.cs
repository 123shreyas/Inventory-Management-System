using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;


namespace InventoryManagementSystem.ConsoleUI
{
    public class ProductConsole
    {
        private readonly ProductService _productService;

        public ProductConsole(ProductService productService)
        {
            _productService = productService;
        }

        public void ShowMenu()
        {
            while (true)
            {
        
                Console.WriteLine("\n--- PRODUCTS ---");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. View Product By ID");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Deactivate Product");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": AddProduct(); break;
                    case "2": ViewAll(); break;
                    case "3": ViewById(); break;
                    case "4": Update(); break;
                    case "5": Deactivate(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine()!;

            Console.Write("Enter SKU: ");
            string sku = Console.ReadLine()!;

            Console.Write("Enter Description: ");
            string description = Console.ReadLine()!;

            int categoryId;
            Console.Write("Enter Category ID: ");
            while (!int.TryParse(Console.ReadLine(), out categoryId))
                Console.Write("Invalid input. Enter valid Category ID: ");

            Console.Write("Enter Unit of Measure: ");
            string uom = Console.ReadLine()!;

            decimal cost;
            Console.Write("Enter Cost: ");
            while (!decimal.TryParse(Console.ReadLine(), out cost))
                Console.Write("Invalid input. Enter valid Cost: ");

            decimal price;
            Console.Write("Enter List Price: ");
            while (!decimal.TryParse(Console.ReadLine(), out price))
                Console.Write("Invalid input. Enter valid Price: ");

            _productService.AddProduct(new Product
            {
                ProductName = name,
                SKU = sku,
                Description = description,
                CategoryId = categoryId,
                UnitOfMeasure = uom,
                Cost = cost,
                ListPrice = price,
                IsActive = true
            });
        }

        private void ViewAll()
        {
            var products = _productService.GetAllProducts();

            Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-10} {4,-10}",
                "ID", "Name", "SKU", "Cost", "Price");

            foreach (var p in products)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-12} {3,-10:F2} {4,-10:F2}",
                    p.ProductId,
                    p.ProductName,
                    p.SKU,
                    p.Cost,
                    p.ListPrice);
            }
        }

        private void ViewById()
{
    int id;
    Console.Write("Enter Product ID: ");
    while (!int.TryParse(Console.ReadLine(), out id))
        Console.Write("Invalid input. Enter valid ID: ");

    var product = _productService.GetProduct(id);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    Console.WriteLine("\n========= PRODUCT DETAILS =========");
    Console.WriteLine($"ID: {product.ProductId}");
    Console.WriteLine($"Name: {product.ProductName}");
    Console.WriteLine($"SKU: {product.SKU}");
    Console.WriteLine($"Description: {product.Description}");
    Console.WriteLine($"Category: {product.Category?.CategoryName ?? "N/A"}");
    Console.WriteLine($"Unit Of Measure: {product.UnitOfMeasure}");
    Console.WriteLine($"Cost: {product.Cost:F2}");
    Console.WriteLine($"List Price: {product.ListPrice:F2}");
    Console.WriteLine($"Active: {product.IsActive}");
    Console.WriteLine("====================================");
}

        private void Update()
{
    Console.Write("Enter Product ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var product = _productService.GetProduct(id);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    Console.Write($"Enter New Name ({product.ProductName}): ");
    string? name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name))
        product.ProductName = name;

    Console.Write($"Enter New Description ({product.Description}): ");
    string? description = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(description))
        product.Description = description;

    Console.Write($"Enter New Cost ({product.Cost}): ");
    if (decimal.TryParse(Console.ReadLine(), out decimal cost))
        product.Cost = cost;

    Console.Write($"Enter New List Price ({product.ListPrice}): ");
    if (decimal.TryParse(Console.ReadLine(), out decimal price))
        product.ListPrice = price;

    Console.Write($"Enter New Category ID ({product.CategoryId}): ");
    if (int.TryParse(Console.ReadLine(), out int category))
        product.CategoryId = category;

    Console.Write($"Enter New Unit Of Measure ({product.UnitOfMeasure}): ");
    string? uom = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(uom))
        product.UnitOfMeasure = uom;

    Console.Write("Set Active? (Y/N): ");
string? input = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(input))
{
    product.IsActive = input.ToUpper() == "Y";
}

    bool updated = _productService.UpdateProduct(product);

    Console.WriteLine(updated 
        ? "Product updated successfully!" 
        : "Update failed.");
}

        private void Deactivate()
        {
            int id;
            Console.Write("Enter Product ID to deactivate: ");
            while (!int.TryParse(Console.ReadLine(), out id))
                Console.Write("Invalid input. Enter valid ID: ");

            _productService.DeleteProduct(id);
        }

         private void Pause()
        {
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}