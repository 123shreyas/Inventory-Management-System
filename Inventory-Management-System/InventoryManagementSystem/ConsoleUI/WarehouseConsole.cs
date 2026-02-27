using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.ConsoleUI
{
    public class WarehouseConsole
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseConsole(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        public void ShowMenu()
        {
            while (true)
            {

                Console.WriteLine("\n--- WAREHOUSES ---");
                Console.WriteLine("1. Add Warehouse");
                Console.WriteLine("2. View All Warehouses");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddWarehouse();
                        break;

                    case "2":
                        ViewAllWarehouses();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddWarehouse()
        {
            Console.Write("Enter Warehouse Name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter Location: ");
            string? location = Console.ReadLine();

            Console.Write("Enter Capacity: ");
            string? capacityInput = Console.ReadLine();

            if (!int.TryParse(capacityInput, out int capacity))
            {
                Console.WriteLine("Invalid capacity. Must be a number.");
                return;
            }

            var warehouse = new Warehouse
            {
                WarehouseName = name,
                Location = location,
                Capacity = capacity
            };

            _warehouseService.AddWarehouse(warehouse);
        }

        private void ViewAllWarehouses()
        {
            var warehouses = _warehouseService.GetAllWarehouses();

            if (!warehouses.Any())
            {
                Console.WriteLine("No warehouses found.");
                return;
            }

            foreach (var w in warehouses)
            {
                Console.WriteLine(
                    $"ID: {w.WarehouseId}, " +
                    $"Name: {w.WarehouseName}, " +
                    $"Location: {w.Location}, " +
                    $"Capacity: {w.Capacity}"
                );
            }
        }
    }
}