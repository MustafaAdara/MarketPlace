using Marketplace.Models;

namespace Marketplace.Interfaces
{
    public interface IMarketRepository
    {
        ICollection<Market> GetMarkets();

        Market GetMarket(int id);

        bool MarketExist(int id);

        bool Save();

        bool CreateMarket(Market market);
        bool UpdateMarket(Market market);

        bool DeleteMarket(Market market);


    }
}
