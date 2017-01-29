using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Model;

namespace SelfCheckout.Kiosk.Controller
{
    public class Purchase
    {
        public IEnumerable<Item> BuyItems { get; set; }

        public decimal Total => BuyItems.Sum(x => x.Price);

        public Purchase(IEnumerable<Item> buyItems)
        {
            BuyItems = buyItems;
        }

        public void ApplySales(IEnumerable<Sale> sales)
        {
            foreach (Sale sale in sales)
            {
                ApplySale(sale);
            }
        }

        public void ApplySale(Sale sale)
        {
            switch (sale.SaleType)
            {
                case SaleType.OnSale:
                    ApplyOnSaleSale(sale);
                    break;

                case SaleType.Group:
                    ApplyGroupSale(sale);
                    break;

                case SaleType.AdditionalProduct:
                    ApplyAdditionalProductSale(sale);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyOnSaleSale(Sale sale)
        {
            foreach (Item item in BuyItems.Where(x => x.Name == sale.ItemName))
            {
                var salePrice = Math.Round(Convert.ToDecimal(sale.SalePrice), 2);
                if (item.Price > salePrice)
                    item.Price = salePrice;
            }
        }

        private void ApplyGroupSale(Sale sale)
        {
            List<Item> groupedItems =
                BuyItems.Where(x => x.Name == sale.ItemName).ToList();
            
            if (sale.NumRequired == 0)
                return;

            var total = groupedItems.Count()/sale.NumRequired;
            var discount = Math.Round(Convert.ToDecimal(sale.SalePrice / sale.NumRequired), 2); 

            for (int i = 0; i < total*sale.NumRequired; i ++)
            {
                if (groupedItems[i].Price > discount)
                    groupedItems[i].Price = discount;
            }
        }

        private void ApplyAdditionalProductSale(Sale sale)
        {
            List<Item> groupedItems =
                BuyItems.Where(x => x.Name == sale.ItemName).OrderByDescending(x => x.Price).ToList();
            
            if (sale.NumRequired == 0)
                return;

            var total = groupedItems.Count()/(sale.NumRequired+1);
            
            for (int i = 0; i < total; i++)
            {
                groupedItems[i].Price = Math.Round(Convert.ToDecimal(groupedItems[i].Price * (decimal) sale.ItemDiscount), 2);
            }
        }
    }
}
