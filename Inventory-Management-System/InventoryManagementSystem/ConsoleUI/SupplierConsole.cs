using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.ConsoleUI
{
    public class SupplierConsole
    {
        private readonly SupplierService _supplierService;

        public SupplierConsole(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                
                Console.WriteLine("\n--- SUPPLIERS ---");
                Console.WriteLine("1. Add Supplier");
                Console.WriteLine("2. View All Suppliers");
                Console.WriteLine("3. Link Product To Supplier");
                Console.WriteLine("4. View Supplier Products");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddSupplier();
                        break;

                    case "2":
                        ViewAllSuppliers();
                        break;

                    case "3":
                        LinkProductToSupplier();
                        break;

                    case "4":
                        ViewSupplierProducts();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddSupplier()
        {
            Console.Write("Enter Supplier Name: ");
            string name = Console.ReadLine()!;

            Console.Write("Enter Email (optional): ");
            string? email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) email = null;

            Console.Write("Enter Phone (optional): ");
            string? phone = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phone)) phone = null;

            Console.Write("Enter Website (optional): ");
            string? website = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(website)) website = null;

            var supplier = new Supplier
            {
                SupplierName = name,
                Email = email,
                Phone = phone,
                Website = website
            };

            _supplierService.AddSupplier(supplier);

            Console.WriteLine("Supplier added successfully!");
        }

        private void ViewAllSuppliers()
        {
            var suppliers = _supplierService.GetAllSuppliers();

            if (!suppliers.Any())
            {
                Console.WriteLine("No suppliers found.");
                return;
            }

            foreach (var s in suppliers)
            {
                Console.WriteLine(
                    $"ID: {s.SupplierId}, " +
                    $"Name: {s.SupplierName}, " +
                    $"Email: {s.Email ?? "-"}, " +
                    $"Phone: {s.Phone ?? "-"}, " +
                    $"Website: {s.Website ?? "-"}"
                );
            }
        }

        private void LinkProductToSupplier()
        {
            Console.Write("Enter Product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID.");
                return;
            }

            Console.Write("Enter Supplier ID: ");
            if (!int.TryParse(Console.ReadLine(), out int supplierId))
            {
                Console.WriteLine("Invalid Supplier ID.");
                return;
            }

            Console.Write("Enter Supplier SKU (optional): ");
            string? supplierSku = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(supplierSku))
                supplierSku = null;

            Console.Write("Enter Lead Time (days): ");
            if (!int.TryParse(Console.ReadLine(), out int leadTime))
            {
                Console.WriteLine("Invalid Lead Time.");
                return;
            }

            _supplierService.AddProductSupplier(new ProductSupplier
            {
                ProductId = productId,
                SupplierId = supplierId,
                SupplierSKU = supplierSku,
                LeadTime = leadTime
            });

            Console.WriteLine("Product linked to supplier successfully!");
        }

        private void ViewSupplierProducts()
        {
            Console.Write("Enter Supplier ID: ");
            if (!int.TryParse(Console.ReadLine(), out int supplierId))
            {
                Console.WriteLine("Invalid Supplier ID.");
                return;
            }

            var products = _supplierService.GetProductsBySupplier(supplierId);

            if (!products.Any())
            {
                Console.WriteLine("No products found for this supplier.");
                return;
            }

            foreach (var ps in products)
            {
                Console.WriteLine(
                    $"Product: {ps.Product.ProductName}, " +
                    $"Supplier SKU: {ps.SupplierSKU ?? "-"}, " +
                    $"Lead Time: {ps.LeadTime} days"
                );
            }
        }
    }
}