using Marketplace.Models;

namespace Marketplace.Interfaces
{
    public interface IShoppingCartRepository
    {
        ICollection<ShoppingCart> GetCarts();

        ShoppingCart GetCart(int id);
        ShoppingCart GetCartByUser(int userId);
        bool CartExists(int id);
        bool CreateCart(ShoppingCart cart);
        bool UpdateCart(ShoppingCart cart);
        bool DeleteCart(ShoppingCart cart);
        bool Save();
    }
}
