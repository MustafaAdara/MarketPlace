using Marketplace.Data;
using Marketplace.Interfaces;
using Marketplace.Models;

namespace Marketplace.Repository
{
    public class MarketRepository : IMarketRepository
    {
        private readonly DataContext _context;

        public MarketRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateMarket(Market market)
        {
            _context.Add(market);
            return Save();
        }

        public bool DeleteMarket(Market market)
        {
            _context.Remove(market);
            return Save();
        }

        public Market GetMarket(int markId)
        {
            return _context.Markets.Where(m=> m.Id == markId).FirstOrDefault();
        }

        public ICollection<Market> GetMarkets()
        {
            return _context.Markets.ToList();
        }

        public bool MarketExist(int id)
        {
            return _context.Markets.Any(m => m.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMarket(Market market)
        {
            _context.Update(market);
            return Save();
        }
    }
}
