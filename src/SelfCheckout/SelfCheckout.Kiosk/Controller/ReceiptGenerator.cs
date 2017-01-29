using System;
using System.Collections.Generic;
using System.Text;
using SelfCheckout.Model;
using System.Linq;

namespace SelfCheckout.Kiosk.Controller
{
    public static class ReceiptGenerator
    {
        public static string GenerateReciept(Purchase purchase)
        {
            StringBuilder builder = new StringBuilder();

            List<Tuple<string, string, string, string>> itemStrings = new List<Tuple<string, string, string, string>>();

            foreach (Item item in purchase.BuyItems.OrderBy(i => i.Name))
            {
                string isDiscounted = item.Price != item.BasePrice ? "\u2713" : string.Empty;

                itemStrings.Add(new Tuple<string, string, string, string>(item.Name,
                    item.BasePrice.ToString("c"), item.Price.ToString("c"), isDiscounted));
            }

            builder.AppendLine(itemStrings.ToStringTable(
                new[] {"Item Name", "Price", "Sale Price", "Discounted"},
                i => i.Item1, i => i.Item2, i => i.Item3, i => i.Item4));

            builder.Append($"TOTAL: {purchase.Total:c}");

            return builder.ToString();
        }
    }
}
