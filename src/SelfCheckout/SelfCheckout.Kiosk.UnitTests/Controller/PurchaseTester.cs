using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.UnitTests.Controller
{
    [TestFixture]
    public class PurchaseTester
    {
        #region Test Data Collections

        private IRepository _repository;

        private string _repositoryFolderName;

        [SetUp]
        public void SetUp()
        {
            _repositoryFolderName = Path.Combine("C:\\ShoppingList\\",
                "SelfCheckout.UnitTests");

            _repository = new GenericRepository(_repositoryFolderName);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_repositoryFolderName, true);
        }

        public static IEnumerable<string> ItemNames
        {
            get
            {
                yield return "apple";
                yield return "banana";
                yield return "carrot";
            }
        }

        public static IEnumerable<Item> TestItems
        {
            get
            {
                yield return new Item("apple", 0.52m);
                yield return new Item("banana", 4.56m);
                yield return new Item("carrot", 1.01m);
            }
        }

        public static IEnumerable<Item> TestCart1
        {
            get
            {
                List<Item> items = new List<Item>();

                items.AddRange(GetTestItem("apple", 2));
                items.AddRange(GetTestItem("banana", 2));
                items.AddRange(GetTestItem("carrot", 2));

                return items;
            }
        }

        public static IEnumerable<Item> TestCart2
        {
            get
            {
                List<Item> items = new List<Item>();

                items.AddRange(GetTestItem("apple", 2));
                items.AddRange(GetTestItem("banana", 3));
                items.AddRange(GetTestItem("carrot", 4));

                return items;
            }
        }

        public static IEnumerable<Item> TestCart3
        {
            get
            {
                List<Item> items = new List<Item>();

                items.AddRange(GetTestItem("apple", 5));
                items.AddRange(GetTestItem("banana", 6));
                items.AddRange(GetTestItem("carrot", 7));

                return items;
            }
        }

        public static IEnumerable<IEnumerable<Item>> TestCarts
        {
            get
            {
                yield return new List<Item>(TestCart1);
                yield return new List<Item>(TestCart2);
                yield return new List<Item>(TestCart3);
            }
        }
        
        #endregion

        #region Helpers

        private static Item GetTestItem(string name)
        {
            return TestItems.Single(g => g.Name == name);
        }

        private static IEnumerable<Item> GetTestItem(string name, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Item(TestItems.Single(x => x.Name == name));
            }
        }

        private static decimal GetRandomDecimal(int min, int max)
        {
            Random random = new Random();

            double next = random.NextDouble();

            return new decimal(min + next*(max - min));
        }

        #endregion

        [Test]
        public void purchase_items_discount_price_is_same_as_regular_price_by_default()
        {
            IEnumerable<Item> items = TestItems.ToList();

            Purchase purchase = new Purchase(items.Select(x => new Item(x)));

            for (int i = 0; i < items.Count(); i++)
            {
                Assert.AreEqual(items.ElementAt(i).Price, purchase.BuyItems.ElementAt(i).Price);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCarts))]
        public void applying_a_sale_sets_price_for_all_items_of_that_type_in_purchase(
            IEnumerable<Item> cart)
        {
            decimal salePrice = new Random().Next(4) + Math.Round(GetRandomDecimal(0, 1), 2);

            List<Sale> sales = new List<Sale>();

            foreach (string itemName in ItemNames)
            {
                sales.Add(new Sale(itemName, salePrice));
            }

            Purchase purchase = new Purchase(cart);
            purchase.ApplySales(sales);

            foreach (string itemName in ItemNames)
            {
                IEnumerable<Item> items = purchase.BuyItems.Where(x => x.Name == itemName);

                Assert.That(items.All(a => a.Price == salePrice || a.BasePrice < salePrice));
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCarts))]
        public void group_sale_sets_price_for_a_set_of_required_items(IEnumerable<Item> sampleCart)
        {
            decimal salePrice = new Random().Next(4) + Math.Round(GetRandomDecimal(0, 1), 2);
            int itemsRequired = new Random().Next(1, 5);

            List<Sale> sales = new List<Sale>();

            foreach (string itemName in ItemNames)
            {
                sales.Add(new Sale(GetTestItem(itemName).Name, salePrice, itemsRequired));
            }

            Purchase purchase = new Purchase(sampleCart);
            purchase.ApplySales(sales);

            foreach (string itemName in ItemNames)
            {
                int applicableItemsCount = purchase.BuyItems.Count(x => x.Name == itemName);
                int discountedItemsCount = purchase.BuyItems.Count(p => (p.Name == itemName) && (p.Price == Math.Round(salePrice/itemsRequired,2) || p.BasePrice < Math.Round(salePrice / itemsRequired, 2)));
                 
                Assert.That((discountedItemsCount == 0) || (applicableItemsCount % discountedItemsCount < itemsRequired));
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCarts))]
        public void additional_product_sale_sets_price_for_the_highest_price_item_of_that_type(
            IEnumerable<Item> sampleCart)
        {
            double salePrice = (double) Math.Round(GetRandomDecimal(0, 1), 2);
            int itemsRequired = new Random().Next(1,3);

            List<Sale> sales = new List<Sale>();

            foreach (string itemName in ItemNames)
            {
                sales.Add(new Sale(GetTestItem(itemName).Name, itemsRequired, salePrice));
            }

            Purchase purchase = new Purchase(sampleCart);
            purchase.ApplySales(sales);

            foreach (string itemName in ItemNames)
            {
                double applicableItemsCount = purchase.BuyItems.Count(x => x.Name == itemName);
                double discountedItemsCount = purchase.BuyItems.Count(p => (p.Name == itemName) && (p.Price == Math.Round(p.BasePrice*(decimal)salePrice,2)));

                int expectedDiscountedItemsCount = (int) Math.Floor(applicableItemsCount/(itemsRequired + 1));

                Assert.That(expectedDiscountedItemsCount == (int) discountedItemsCount);
            }
        }
    }
}
