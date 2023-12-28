namespace Marketplace.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        public ICollection<Product> Products { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
