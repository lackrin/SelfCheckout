using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.Controller
{
    public static class SaleFactory
    {

        public static void CreateSale(string itemName, decimal salePrice, IRepository repository)
        {
            Sale sale = new Sale(itemName, salePrice);
            repository.Create(sale);
        }

        public static void CreateSale(string itemName, decimal salePrice, int numRequired, IRepository repository)
        {
            Sale sale = new Sale(itemName, salePrice, numRequired);
            repository.Create(sale);
        }

        public static void CreateSale(string itemName, int numRequired, double itemDiscount, IRepository repository)
        {
            Sale sale = new Sale(itemName, numRequired, itemDiscount);
            repository.Create(sale);
        }

        public static void RemoveSale(Sale sale, IRepository repository)
        {
            repository.Delete(sale);
        }

    }
}
