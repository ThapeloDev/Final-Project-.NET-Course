using System.Collections.Concurrent;

namespace BLL
{
    public class Chain : IChain
    {
        public string ChainId { get; private set; }
        public string ChainName { get; private set; }
        public double TotalCartPrice { get; set; }
        public ConcurrentDictionary<string, Item> Items { get; set; }
        public Chain(string chainName, string chainId)
        {
            Items = new ConcurrentDictionary<string, Item>();
            ChainName = chainName;
            ChainId = chainId;
        }
    }
}
