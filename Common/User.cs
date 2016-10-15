using System.Collections.Generic;

namespace BEL
{
    public class User
    {
        public string Username { get; set; }
        public Dictionary<string, string> ItemList { get; set; }
        public User(string username, Dictionary<string, string> itemList)
        {
            Username = username;
            ItemList = itemList;
        }
    }
}
