using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    public class WarehouseService
    {
        private readonly InventoryContext _context;

        public WarehouseService(InventoryContext context)
        {
            _context = context;
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            Console.WriteLine("Warehouse Added!");
        }

        public List<Warehouse> GetAllWarehouses()
        {
            return _context.Warehouses.ToList();
        }
    }
}
