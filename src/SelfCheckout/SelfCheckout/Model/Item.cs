namespace SelfCheckout.Model
{
    public class Item : Entity
    {
        public Item()
        {
        }

        public Item(string name, decimal price)
        {
            Name = name;
            Price = price;
            BasePrice = Price;
        }

        public Item(Item single)
        {
            Name = single.Name;
            Price = single.Price;
            BasePrice = single.BasePrice;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal BasePrice { get; set; }

        public override string ToString() => Name;
    }
}
