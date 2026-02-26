using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ConsoleUI
{
    public class StockTransactionConsole
    {
        private readonly InventoryContext _context;

        public StockTransactionConsole(InventoryContext context)
        {
            _context = context;
        }

        public void ShowMenu()
        {
            while (true)
            {
                
                Console.WriteLine("\n--- STOCK TRANSACTIONS ---");
                Console.WriteLine("1. View All Transactions");
                Console.WriteLine("2. Add Transaction");
                Console.WriteLine("0. Back");
                Console.Write("Select: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ViewAllTransactions();
                        break;

                    case "2":
                        AddTransaction();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void ViewAllTransactions()
        {
            var transactions = _context.StockTransactions
                .Include(t => t.Product)
                .Include(t => t.Warehouse)
                .ToList();

            if (!transactions.Any())
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-10} {4,-10} {5,-20}",
                "ID", "Product", "Warehouse", "Qty", "Type", "Reference");

            foreach (var t in transactions)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-10} {4,-10} {5,-20}",
                    t.TransactionId,
                    t.Product?.ProductName,
                    t.Warehouse?.WarehouseName,
                    t.Quantity,
                    t.TransactionType,
                    t.Reference);
            }
        }

        private void AddTransaction()
        {
            Console.Write("Enter Product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Invalid Product ID.");
                return;
            }

            Console.Write("Enter Warehouse ID: ");
            if (!int.TryParse(Console.ReadLine(), out int warehouseId))
            {
                Console.WriteLine("Invalid Warehouse ID.");
                return;
            }

            Console.Write("Enter Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid Quantity.");
                return;
            }

            Console.Write("Enter Transaction Type (IN, OUT, ADJUSTMENT): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out TransactionType type))
            {
                Console.WriteLine("Invalid Transaction Type.");
                return;
            }

            Console.Write("Enter Reference (optional): ");
            string? reference = Console.ReadLine();

            var transaction = new StockTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                TransactionType = type,
                Reference = reference,
                TransactionDate = DateTime.Now
            };

            _context.StockTransactions.Add(transaction);
            _context.SaveChanges();

            Console.WriteLine("Transaction added successfully.");
        }
    }
}