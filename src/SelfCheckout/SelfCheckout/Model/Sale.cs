namespace SelfCheckout.Model
{
    public class Sale : Entity
    {
        public string ItemName { get; set; }

        public SaleType SaleType { get; set; }

        public decimal SalePrice { get; set; }

        public int NumRequired { get; set; }
        
        public double ItemDiscount { get; set; }

        public Sale()
        {
        }

        public Sale(string itemName, decimal salePrice)
        {
            ItemName = itemName;
            SaleType = SaleType.OnSale;
            SalePrice = salePrice;
            NumRequired = 1;
        }

        public Sale(string itemName, decimal salePrice, int numRequired)
        {
            ItemName = itemName;
            SaleType = SaleType.Group;
            SalePrice = salePrice;
            NumRequired = numRequired;
        }

        public Sale(string itemName, int numRequired, double itemDiscount)
        {
            ItemName = itemName;
            SaleType = SaleType.AdditionalProduct;
            NumRequired = numRequired;
            ItemDiscount = itemDiscount;
        }
    }

    public enum SaleType
    {
        OnSale = 1,
        Group = 2,
        AdditionalProduct = 3
    }
}
