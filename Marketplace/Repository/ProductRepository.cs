using Marketplace.Data;
using Marketplace.Interfaces;
using Marketplace.Models;

namespace Marketplace.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateProduct(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public ICollection<Product> GetProductByCategory(int categoryId)
        {
            return _context.Products.Where(p => p.Category.Id == categoryId).ToList();
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.OrderBy(c => c.Id).ToList();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool ProductExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}
