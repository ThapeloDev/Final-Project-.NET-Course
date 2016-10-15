using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Client.Entities;
using Server.Interfaces;

namespace Server.Entities
{
    public class XmlParsing : IXmlParsing
    {
        private const string _path = @"C:\Users\CodeValue\Desktop\Final Project\DataBase\";
        
        public void BuildDataBase()
        {
            string[] filePaths = Directory.GetFiles(_path, "*.xml");
            Parallel.ForEach(filePaths, BuildStoresAndItems);
            
            /*for debug*/
            Savetofile();
        }

        private void BuildStoresAndItems(string file)
        {
            var xDoc = XDocument.Load(file);
            var store = new Store
            {
                StoreName = StoreIdToStoreName.GetStoreName((string)xDoc.Root.Element("ChainId")),
                StoreId = (string)xDoc.Root.Element("StoreId"),
                ProductCount = int.Parse(xDoc.Root.Element("Items").Attribute("Count").Value)
            };

            var items = (from p in xDoc.Root.Element("Items").Elements("Item")
                         select new Item()
                         {
                             ItemName = p.Element("ItemName").Value,
                             ItemPrice = float.Parse(p.Element("ItemPrice").Value)
                         }).ToList();

            store.Items = items;
            DataBase.MyDB.Add(store);
            
        }

        private void Savetofile()
        {
            var i = 0;
            foreach (var item in DataBase.MyDB)
            {
                File.WriteAllText(_path + item.StoreName + i + ".txt", item.ToString());
                i++;
            }
        }
    }
}
