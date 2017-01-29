using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.Controller
{
    public class Checkout
    {
        private readonly IRepository _repository;

        public Checkout(IRepository repository)
        {
            _repository = repository;
        }

        public void StartCheckout()
        {
            IEnumerable<Item> allItems = _repository.GetAll<Item>().ToList();
            IEnumerable<Sale> sales = _repository.GetAll<Sale>().ToList();

            List<Item> boughtItems = new List<Item>();
            List<Sale> effSales = new List<Sale>();

            IEnumerable<string> cartItems = GetItemsInCart();

            foreach (string cartItem in cartItems)
            {
                Item boughtItem = allItems.FirstOrDefault(i => i.Name == cartItem);
                boughtItems.Add(boughtItem != null
                    ? new Item(boughtItem.Name, boughtItem.Price)
                    : new Item("ITEM NOT FOUND:" + cartItem, 0.00M));

                var salesPerItem = sales.ToList().FindAll(p => p.ItemName == cartItem);
                foreach (var sale in salesPerItem)
                {
                    if (!effSales.Exists(p => p.Id == sale.Id))
                        effSales.Add(sale);
                }
            }

            DoCheckout(boughtItems, effSales);
        }

        private IEnumerable<string> GetItemsInCart()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() != DialogResult.OK)
                return Enumerable.Empty<string>();

            string cartContents;

            using (Stream stream = dialog.OpenFile())
            using (StreamReader reader = new StreamReader(stream))
            {
                cartContents = reader.ReadToEnd();
            }

            return cartContents.Split(',');
        }

        public static void DoCheckout(IEnumerable<Item> boughtItems, IEnumerable<Sale> effSales)
        {
            Purchase purchase = new Purchase(boughtItems);

            purchase.ApplySales(effSales);

            DisplayReciept(purchase);
        }

        private static void DisplayReciept(Purchase purchase)
        {
            string receiptPath = Path.Combine(ConfigurationManager.AppSettings["RepositoryFolder"], "receipt.txt");
            
            using (StreamWriter file = new StreamWriter(receiptPath))
            {
                file.Write(ReceiptGenerator.GenerateReciept(purchase));
            }

            Process.Start(receiptPath);
        }
    }
}
