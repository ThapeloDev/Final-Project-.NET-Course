using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BEL;
using BLL;

namespace DAL
{
    public static class Server
    {
        private const string PATH = @"C:\Users\CodeValue\Desktop\Final Project\DAL\Database\";
        private const string ITEMS_FILE = "CommonItems.xml";
        private const string CHAINS_FILE = "Chains.xml";
        private const string USER_FILE = "Users.xml";

        public static Dictionary<string, List<string>> GetAllCommonItems()
        {
            var itemsList = new Dictionary<string, List<string>>();
            var xDoc = XDocument.Load(PATH + ITEMS_FILE);
            xDoc.Root?.Element("Categories")?.Elements("Category").ToList().ForEach(category =>
            {
                var lst = category.Elements("Item").Select(item => item?.Element("ItemName")?.Value).ToList();
                itemsList.Add(category.Attribute("name").Value, lst);
            });
            return itemsList;
        }
        public static void ParseUserChoice(Dictionary<string,double> userItems, Dictionary<string, IChain> chainList)
        {
            var xDoc = XDocument.Load(PATH + ITEMS_FILE);
            xDoc.Root?.Element("Categories")?.Elements("Category")
                .AsParallel().ForAll(category =>
            {
                category.Elements("Item")
                    .AsParallel()
                    .Where(item => userItems.ContainsKey(item?.Element("ItemName")?.Value)).ToList()
                    .ForEach(item =>
                    {
                        InsertItemPrice(item, item.Elements("ItemCode").ToList(), chainList, userItems);
                    });
            });
        }
        private static void InsertItemPrice(XContainer item, IEnumerable<XElement> toList, IReadOnlyDictionary<string, IChain> chainList, IReadOnlyDictionary<string, double> userItems)
        {
           toList.ToList().ForEach(itemCode =>
             {
                 var newItem = new Item(item.Element("ItemName")?.Value, userItems[item?.Element("ItemName")?.Value]);
                 AddItemToChain(itemCode.Attribute("name").Value, 
                     itemCode.Attribute("code").Value,
                     newItem,
                     chainList);
             });
        }
        private static void AddItemToChain(string chainName, string itemCode,Item newItem, IReadOnlyDictionary<string, IChain> chainList)
        {
            var t = chainList[chainName];
            var tt = t.Items;
            tt.TryAdd(itemCode, newItem);
        }
        public static Dictionary<string,IChain> GetChains()
        {
            var xDoc = XDocument.Load(PATH + CHAINS_FILE);
            return xDoc.Root?.Elements("Chain").ToDictionary(item => item.Attribute("name").Value,item => 
                   new Chain(item.Attribute("name").Value,  item.Attribute("code").Value) as IChain );
        }
        public static void GetPrices(Dictionary<string, IChain> chains)
        {
            double itemPrice;
             foreach (var chain in chains)
            {
                chain.Value.TotalCartPrice = 0;
                 var xDoc = XDocument.Load(PATH + chain.Value.ChainName + ".xml");
                foreach (var item in chain.Value.Items)
                {
                    item.Value.Price =
                        (xDoc.Root?.Elements().Last().Elements()
                            .Where(it => it.Element("ItemCode")?.Value == item.Key)
                            .Select(price => price.Element("ItemPrice")?.Value).ToList().First());

                    if (double.TryParse(item.Value?.Price, out itemPrice))
                    {
                        chain.Value.TotalCartPrice += itemPrice*item.Value.Quantity;
                    }
                }
            }
        }
        public static User GetUser(string username)
        {
            var xDoc = XDocument.Load(PATH + USER_FILE);

            var userElem = xDoc.Root?.Element("Users")
                .Elements("User")
                .Where(xuser => xuser.Attribute("name").Value == username);

            if (!userElem.Any()) return null;

            var itemList = userElem.Elements("Items").Elements("Item").ToDictionary(xitem => xitem.Value, xitem => xitem.FirstAttribute.Value);

            return new User(username, itemList);
        }
        public static bool SignNewUser(string username)
        {
            try
            {
                var xDoc = XDocument.Load(PATH + USER_FILE);

                var user = xDoc.Element("DataBase").Element("Users");
                user.Add(new XElement("User",
                           new XAttribute("name", username),
                           new XElement("Items")));
                xDoc.Save(PATH + USER_FILE);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static void SaveUserFavoriteCart(string username, Dictionary<string, double> cart)
        {
            var xDoc = XDocument.Load(PATH + USER_FILE);

            var user = xDoc.Element("DataBase")?.Element("Users")?.Elements("User")
                .Where(xuser => xuser.Attribute("name").Value == username)
                .First()
                .Element("Items");

            user.Elements("Item").ToList().ForEach(item => item.Remove());

            foreach (var item in cart)
            {
                user.Add(new XElement("Item",
                         new XAttribute("amount", item.Value), item.Key));
            }
            
            xDoc.Save(PATH + USER_FILE);
        }
    }
}
