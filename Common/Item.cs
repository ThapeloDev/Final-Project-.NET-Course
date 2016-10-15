namespace BLL
{
    public class Item
    {
        public string ItemName { get; private set; }
        public double Quantity { get; private set; }
        public string Price { get; set; }
        public Item(string itemName, double quantity)
        {
            ItemName = itemName;
            Quantity = quantity;
        }
    }
}
