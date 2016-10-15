using Server.Interfaces;

namespace Server.Entities
{
    public class Item : IItem
    {
        public string ItemName { get; set; }
        public float ItemPrice { get; set; }
    }
}
