using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class ShoppingCartLogic
    {
        public Dictionary<string, IChain> Chains { get; private set; }

        public ShoppingCartLogic()
        {
            Chains = Server.GetChains();
        }
        public Task<Dictionary<string, List<string>>> GetAllItemsFromDb()
        {
            return Task.Run(() => Server.GetAllCommonItems());
        }
        public void DeleteAllItemsFromChains()
        {
            foreach (var chain in Chains.Values)
            {
                chain.Items.Clear();
            }
        }
        public void GetCartPrice(Dictionary<string,double> userItems)
        {
            Server.ParseUserChoice(userItems, Chains);
             CartPriceInChain();
        }
        public string GetChainTotalCartPrice(string chainName)
        {
            return Chains[chainName].TotalCartPrice.ToString();
        }
        
        public void CartPriceInChain()
        {
            Server.GetPrices(Chains);
        }
        public string[] GetChainItemsOrderByPrice(string chainName)
        {
            var res = new StringBuilder();
            var itemsInOrder = Chains[chainName].Items.Values.ToList().OrderBy(item => double.Parse(item.Price)).ToList();

            itemsInOrder.ForEach(item => res.Append(item.ItemName +" "+item.Price + Environment.NewLine));

            return res.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }
        public string[] GetChainItemsOrderByPriceDescending(string chainName)
        {
            var res = new StringBuilder();
            var itemsInOrder = Chains[chainName].Items.Values.ToList().OrderByDescending(item => double.Parse(item.Price)).ToList();

            itemsInOrder.ForEach(item => res.Append(item.ItemName + " " + item.Price + Environment.NewLine));

            return res.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }
        public override string ToString()
        {
            var str= new StringBuilder();
            foreach (var chain in Chains)
            {
                str.Append(chain.Value.ChainName);
                foreach (var item in chain.Value.Items)
                {
                    str.Append(" " + item.Value.ItemName + " " + item.Value.Quantity);
                }
                str.Append(Environment.NewLine);
            }
            return str.ToString();
        }
    }
}
