using System;
using System.Collections.Generic;
using System.Text;
using Server.Entities;
using Server.Interfaces;

namespace Client.Entities
{
    public class Store : IStore
    {
        public string ChainId { get; set; }
        public string StoreId { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public string StoreName { get; set; }
        public DateTime PriceUpdateDate { get; set; }
        public int SubChainId { get; set; }
        public int ProductCount { get; set; }

        public override string ToString()
        {
            StringBuilder toPrint = new StringBuilder();
            toPrint.Append("Store name: " + StoreName + Environment.NewLine);

            foreach (var VARIABLE in Items)
            {
                toPrint.Append(VARIABLE.ItemName + Environment.NewLine);
            }

            return toPrint.ToString();
        }
    }
}
