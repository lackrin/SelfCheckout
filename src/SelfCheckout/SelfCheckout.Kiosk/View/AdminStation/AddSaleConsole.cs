using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.View.AdminStation
{
    public class AddSaleConsole
    {
        private readonly IRepository _repository;

        public AddSaleConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            Item forSale = SelectItem();

            Console.WriteLine("Select the type of sale you want to create...");

            SaleType type = ConsoleHelper.SelectFrom(Enum.GetValues(typeof(SaleType)).Cast<SaleType>());
            
            switch (type)
            {
                case SaleType.OnSale:
                    AddOnSaleSale(forSale);
                    break;
                case SaleType.Group:
                    AddGroupSale(forSale);
                    break;
                case SaleType.AdditionalProduct:
                    AddAdditionalProductSale(forSale);
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    return;
            }

            Console.WriteLine($"Sale added for {forSale.Name}.");
        }

        private void AddOnSaleSale(Item item)
        {
            decimal salePrice = GetSalePrice();
            SaleFactory.CreateSale(item.Name, salePrice, _repository);
        }

        private void AddGroupSale(Item item)
        {
            int numRequired = GetNumRequired();
            decimal salePrice = GetSalePrice();

            SaleFactory.CreateSale(item.Name, salePrice, numRequired, _repository);
        }

        private void AddAdditionalProductSale(Item item)
        {
            int numRequired = GetNumRequired();
            double discount = ConsoleHelper.GetDouble("Enter the discount (%) for this sale:");

            if (discount > 1)
                discount = discount/100;

            SaleFactory.CreateSale(item.Name, numRequired, discount, _repository);
            
        }

        private Item SelectItem()
        {
            Console.WriteLine("Select the item for this sale:");

            IEnumerable<Item> items = _repository.GetAll<Item>().ToList();

            return ConsoleHelper.SelectFrom(items);
        }

        private int GetNumRequired()
        {
            int result = 0;
            while (result <= 0)
            {
                result = ConsoleHelper.GetInt("Enter the number of items required (greater than 0):");
                if(result <=0)
                    Console.WriteLine("Invalid Entry: Items must be greater then 0.");
            }
            return result;
        }

        private decimal GetSalePrice()
        {
            return ConsoleHelper.GetDecimal("Enter the sale price (0.00):");
        }
    }
}
