using System.Collections.Concurrent;

namespace BLL
{
    public interface IChain
    {
        double TotalCartPrice { get; set; }
        ConcurrentDictionary<string, Item> Items { get; set; }
        string ChainName { get;}
        string ChainId { get;}
    }
}