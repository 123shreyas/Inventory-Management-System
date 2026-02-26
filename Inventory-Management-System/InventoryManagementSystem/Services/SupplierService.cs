using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    public class SupplierService
    {
        private readonly InventoryContext _context;

        public SupplierService(InventoryContext context)
        {
            _context = context;
        }

        public void AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public void AddProductSupplier(ProductSupplier productSupplier)
        {
            _context.ProductSuppliers.Add(productSupplier);
            _context.SaveChanges();
        }

        public List<ProductSupplier> GetProductsBySupplier(int supplierId)
        {
            return _context.ProductSuppliers
                .Include(ps => ps.Product)
                .Where(ps => ps.SupplierId == supplierId)
                .ToList();
        }
    }
}