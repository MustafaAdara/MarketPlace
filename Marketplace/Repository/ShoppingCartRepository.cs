using Marketplace.Data;
using Marketplace.Interfaces;
using Marketplace.Models;

namespace Marketplace.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly DataContext _context;

        public ShoppingCartRepository(DataContext context)
        {
            _context = context;
        }
        public bool CartExists(int id)
        {
            return _context.ShoppingCarts.Any(x => x.Id == id);
        }

        public bool CreateCart(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
            return Save();
        }

        public bool DeleteCart(ShoppingCart cart)
        {
            _context.Remove(cart);
            return Save();
        }

        public ShoppingCart GetCart(int id)
        {
            return _context.ShoppingCarts.Where(c => c.Id == id).FirstOrDefault();
        }

        public ShoppingCart GetCartByUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(s => s.cart).FirstOrDefault();
        }

        public ICollection<ShoppingCart> GetCarts()
        {
            return _context.ShoppingCarts.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCart(ShoppingCart cart)
        {
            _context.Update(cart);
            return Save();
        }
    }
}
