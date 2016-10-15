using System;
using System.Collections.Generic;
using Client.Entities;
using Server.Entities;

namespace Server.Interfaces
{
    public interface IStore
    {
        string StoreName { get; set; }
        string ChainId { get; set; }
        int SubChainId { get; set; }
        string StoreId { get; set; }
        IEnumerable<Item> Items { get; set; }
        DateTime PriceUpdateDate { get; set; }
        int ProductCount { get; set; }
    }
}
