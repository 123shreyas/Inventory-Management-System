using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagementSystem.Services
{
    public class StockService
    {
        private readonly InventoryContext _context;

        public StockService(InventoryContext context)
        {
            _context = context;
        }

        public void StockIn(int productId, int warehouseId, int quantity, string reference)
        {
            var transaction = new StockTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                TransactionType = TransactionType.IN,
                Quantity = quantity,
                TransactionDate = DateTime.Now,
                Reference = reference
            };

            _context.StockTransactions.Add(transaction);

            var stock = _context.StockLevels
                .FirstOrDefault(s => s.ProductId == productId && s.WarehouseId == warehouseId);

            if (stock == null)
            {
                stock = new StockLevel
                {
                    ProductId = productId,
                    WarehouseId = warehouseId,
                    QuantityOnHand = quantity,
                    ReorderLevel = 60,
                    SafetyStock = 20
                };
                _context.StockLevels.Add(stock);
            }
            else
            {
                stock.QuantityOnHand += quantity;
            }

            _context.SaveChanges();
            Console.WriteLine("Stock IN Completed!");
        }

        public void StockOut(int productId, int warehouseId, int quantity, string reference)
        {
            var stock = _context.StockLevels
                .FirstOrDefault(s => s.ProductId == productId && s.WarehouseId == warehouseId);

            if (stock == null || stock.QuantityOnHand < quantity)
            {
                Console.WriteLine("Insufficient stock!");
                return;
            }

            var transaction = new StockTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                TransactionType = TransactionType.OUT,
                Quantity = quantity,
                TransactionDate = DateTime.Now,
                Reference = reference
            };

            _context.StockTransactions.Add(transaction);
            stock.QuantityOnHand -= quantity;

            _context.SaveChanges();
            Console.WriteLine("Stock OUT Completed!");
        }

        public void TransferStock(int productId, int fromWarehouseId, int toWarehouseId, int quantity)
        {
            using var dbTransaction = _context.Database.BeginTransaction();

            try
            {
                var sourceStock = _context.StockLevels
                    .FirstOrDefault(s => s.ProductId == productId && s.WarehouseId == fromWarehouseId);

                if (sourceStock == null || sourceStock.QuantityOnHand < quantity)
                {
                    Console.WriteLine("Insufficient stock for transfer!");
                    return;
                }

                // Reduce from source warehouse
                sourceStock.QuantityOnHand -= quantity;

                _context.StockTransactions.Add(new StockTransaction
                {
                    ProductId = productId,
                    WarehouseId = fromWarehouseId,
                    Quantity = quantity,
                    TransactionType = TransactionType.TRANSFER_OUT,
                    TransactionDate = DateTime.Now,
                    Reference = "Transfer Out"
                });

                // Add to destination warehouse
                var destinationStock = _context.StockLevels
                    .FirstOrDefault(s => s.ProductId == productId && s.WarehouseId == toWarehouseId);

                if (destinationStock == null)
                {
                    destinationStock = new StockLevel
                    {
                        ProductId = productId,
                        WarehouseId = toWarehouseId,
                        QuantityOnHand = quantity,
                        ReorderLevel = 60,
                        SafetyStock = 20
                    };

                    _context.StockLevels.Add(destinationStock);
                }
                else
                {
                    destinationStock.QuantityOnHand += quantity;
                }

                _context.StockTransactions.Add(new StockTransaction
                {
                    ProductId = productId,
                    WarehouseId = toWarehouseId,
                    Quantity = quantity,
                    TransactionType = TransactionType.TRANSFER_IN,
                    TransactionDate = DateTime.Now,
                    Reference = "Transfer In"
                });

                _context.SaveChanges();
                dbTransaction.Commit();

                Console.WriteLine("Stock Transfer Completed!");
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public void AdjustStock(int productId, int warehouseId, int newQuantity, string reference)
        {
            var stock = _context.StockLevels
                .FirstOrDefault(s => s.ProductId == productId && s.WarehouseId == warehouseId);

            if (stock == null)
            {
                Console.WriteLine("Stock record not found.");
                return;
            }

            int difference = newQuantity - stock.QuantityOnHand;

            stock.QuantityOnHand = newQuantity;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = difference,
                TransactionType = TransactionType.ADJUST,
                TransactionDate = DateTime.Now,
                Reference = reference
            });

            _context.SaveChanges();

            Console.WriteLine("Stock Adjustment Completed!");
        }

        public void CheckLowStock()
        {
            var lowStock = _context.StockLevels
                .Where(s => s.QuantityOnHand <= s.ReorderLevel)
                .Include(s => s.Product)
                .ToList();

            foreach (var item in lowStock)
            {
                // Console.WriteLine($"Low Stock Alert → ProductId: {item.ProductId}");
                Console.WriteLine($"Low Stock Alert → {item.Product?.ProductName}");

            }
        }


        public decimal GetTotalInventoryValue()
        {
            return _context.StockLevels
                .Select(sl => sl.QuantityOnHand * sl.Product.Cost)
                .Sum();
        }

    }
}
